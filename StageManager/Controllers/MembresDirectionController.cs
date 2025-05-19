using StageManager.DTO.MembreDirectionDTO;
using StageManager.Mapping;
using StageManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestRestApi.Data;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembreDirectionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MembreDirectionController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembreDirectionDto>>> GetMembresDirection()
        {
            var membresDirection = await _context.MembresDirection.ToListAsync();
            return membresDirection.Select(m => m.ToDto()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembreDirectionDto>> GetMembreDirection(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);

            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
            }

            return membreDirection.ToDto();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MembreDirectionDto>> CreateMembreDirection([FromBody] CreateMembreDirectionDto membredirectiondto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingusername = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Username == membredirectiondto.Username);
            if (existingusername != null)
            {
                return BadRequest("Le nom d'utilisateur existe déjà.");
            }

            if (await _context.MembresDirection.AnyAsync(m => m.Email == membredirectiondto.Email))
            {
                return BadRequest($"Un membre de la direction avec l'email {membredirectiondto.Email} existe déjà.");
            }

            string generatedPassword = GenerateRandomPassword(8);
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<MembreDirection>();

            var membreDirection = new MembreDirection
            {
                Nom = membredirectiondto.Nom,
                Prenom = membredirectiondto.Prenom,
                Email = membredirectiondto.Email,
                Telephone = membredirectiondto.Telephone,
                Fonction = membredirectiondto.Fonction,
                Username = membredirectiondto.Username,
                MotDePasse = passwordHasher.HashPassword(null, generatedPassword),
                DatePrisePoste = DateTime.Now,
                EstActif = true,
                DemandesDeStage = new List<DemandeDeStage>(),
                Role = UserRoles.MembreDirection
            };

            // Utilisation d'une transaction pour garantir l'atomicité
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Ajouter le membre de direction à la base de données
                    _context.MembresDirection.Add(membreDirection);
                    await _context.SaveChangesAsync();

                    // Envoyer l'email avec les informations de connexion
                    await EnvoyerEmailAsync(
                        membreDirection.Email,
                        "Vos informations de connexion",
                        $"Bonjour {membreDirection.Prenom},\n\n" +
                        $"Votre compte a été créé avec succès.\n\n" +
                        $"Nom d'utilisateur: {membreDirection.Username}\n" +
                        $"Mot de passe: {generatedPassword}\n\n" +
                        $"Veuillez changer votre mot de passe après votre première connexion.\n\n" +
                        $"Cordialement,\nLe service des stages"
                    );

                    // Valider la transaction seulement si l'email a été envoyé avec succès
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetMembreDirection),
                        new { id = membreDirection.Id },
                        membreDirection.ToDto());
                }
                catch (Exception ex)
                {
                    // En cas d'erreur, annuler la transaction
                    await transaction.RollbackAsync();

                    // Retourner une erreur détaillée
                    return StatusCode(500, $"Une erreur est survenue lors de la création du compte ou de l'envoi de l'email: {ex.Message}");
                }
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz0123456789";
            StringBuilder res = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(validChars[(int)(num % (uint)validChars.Length)]);
                }
            }

            return res.ToString();
        }

        private async Task EnvoyerEmailAsync(string destinataire, string sujet, string corps)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Service des Stages", _configuration["Email:From"]));
                email.To.Add(new MailboxAddress("", destinataire));
                email.Subject = sujet;
                email.Body = new TextPart(TextFormat.Plain) { Text = corps };

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(_configuration["Email:Host"], int.Parse(_configuration["Email:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Rethrow l'exception pour qu'elle soit capturée par la transaction
                throw new Exception($"Erreur lors de l'envoi de l'email: {ex.Message}", ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembreDirection(int id, UpdateMembreDirectionDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("L'ID ne correspond pas");
            }

            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
            }

            if (!string.IsNullOrEmpty(updateDto.Email) &&
                await _context.MembresDirection.AnyAsync(m => m.Email == updateDto.Email && m.Id != id))
            {
                return BadRequest("Cet email est déjà utilisé par un autre membre de la direction");
            }

            if (!string.IsNullOrEmpty(updateDto.Nom))
                membreDirection.Nom = updateDto.Nom;

            if (!string.IsNullOrEmpty(updateDto.Prenom))
                membreDirection.Prenom = updateDto.Prenom;

            if (!string.IsNullOrEmpty(updateDto.Email))
                membreDirection.Email = updateDto.Email;

            if (!string.IsNullOrEmpty(updateDto.Telephone))
                membreDirection.Telephone = updateDto.Telephone;

            if (!string.IsNullOrEmpty(updateDto.Username))
                membreDirection.Username = updateDto.Username;

            if (!string.IsNullOrEmpty(updateDto.Fonction))
                membreDirection.Fonction = updateDto.Fonction;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembreDirectionExists(id))
                {
                    return NotFound("Membre de la direction non trouvé");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPatch("{id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
            }

            membreDirection.EstActif = !membreDirection.EstActif;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembreDirection(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
            }

            _context.MembresDirection.Remove(membreDirection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/DemandesDeStage")]
        public async Task<ActionResult<IEnumerable<DemandeDeStage>>> GetDemandesDeStage(int id)
        {
            if (!MembreDirectionExists(id))
            {
                return NotFound("Membre de la direction non trouvé");
            }

            var demandes = await _context.DemandesDeStage
                .Where(d => d.MembreDirectionId == id)
                .ToListAsync();

            return Ok(demandes);
        }

        [HttpGet("current")]
        public async Task<ActionResult<MembreDirectionDto>> GetCurrentMembreDirection()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
                return NotFound("Membre de direction non trouvé");

            return membreDirection.ToDto();
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
                return NotFound("Membre de direction non trouvé");

            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<MembreDirection>();
            var verificationResult = passwordHasher.VerifyHashedPassword(membreDirection, membreDirection.MotDePasse, model.CurrentPassword);
            if (verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                return BadRequest("Le mot de passe actuel est incorrect");

            membreDirection.MotDePasse = passwordHasher.HashPassword(membreDirection, model.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mot de passe mis à jour avec succès" });
        }

        [HttpPut("update-membre-info")]
        [Authorize(Roles = "MembreDirection")]
        public async Task<IActionResult> UpdateMembreInfo([FromBody] UpdateMembreInfoDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
                return NotFound("Membre de direction non trouvé");

            if (!string.IsNullOrEmpty(updateDto.Email) &&
                await _context.MembresDirection.AnyAsync(m => m.Email == updateDto.Email && m.Id != id))
            {
                return BadRequest("Cet email est déjà utilisé par un autre membre de la direction");
            }

            if (!string.IsNullOrEmpty(updateDto.Email))
                membreDirection.Email = updateDto.Email;

            if (!string.IsNullOrEmpty(updateDto.Telephone))
                membreDirection.Telephone = updateDto.Telephone;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Informations mises à jour avec succès" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembreDirectionExists(id))
                    return NotFound("Membre de direction non trouvé");
                else
                    throw;
            }
        }

        private bool MembreDirectionExists(int id)
        {
            return _context.MembresDirection.Any(e => e.Id == id);
        }
    }
}