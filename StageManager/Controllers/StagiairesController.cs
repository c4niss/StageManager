using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.StagiaireDTO;
using StageManager.Mapping;
using StageManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestRestApi.Data;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Security.Cryptography;
using System.Text;
using System;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagiairesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public StagiairesController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> GetStagiaires()
        {
            var stagiaires = await _db.Stagiaires.ToListAsync();
            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StagiaireDto>> GetStagiaire(int id)
        {
            var stagiaire = await _db.Stagiaires
                .Include(s => s.DemandeDeStage)
                .Include(s => s.Stage)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            return Ok(stagiaire.ToDto());
        }

        [HttpPost]
        [Authorize(Roles = "MembreDirection")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StagiaireDto>> CreateStagiaire([FromBody] StagiaireCreateDto stagiaireDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingusername = await _db.Utilisateurs
                .FirstOrDefaultAsync(u => u.Username == stagiaireDto.Username);
            if (existingusername != null)
            {
                return BadRequest("Le nom d'utilisateur existe déjà.");
            }

            if (await _db.Stagiaires.AnyAsync(s => s.Email == stagiaireDto.Email))
            {
                return BadRequest($"Un stagiaire avec l'email {stagiaireDto.Email} existe déjà.");
            }

            string generatedPassword = GenerateRandomPassword(8);
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Stagiaire>();

            var stagiaire = new Stagiaire
            {
                Nom = stagiaireDto.Nom,
                Prenom = stagiaireDto.Prenom,
                Email = stagiaireDto.Email,
                Telephone = stagiaireDto.Telephone,
                MotDePasse = passwordHasher.HashPassword(null, generatedPassword),
                Universite = stagiaireDto.Universite,
                EstActif = true,
                Username = stagiaireDto.Username,
                Specialite = stagiaireDto.Specialite,
                Status = StagiaireStatus.EnCour,
                DateCreation = DateTime.Now,
                Role = UserRoles.Stagiaire
            };

            // Démarrer une transaction pour assurer l'atomicité
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.Stagiaires.Add(stagiaire);
                await _db.SaveChangesAsync();

                // Envoyer l'email après la création du compte
                try
                {
                    await EnvoyerEmailAsync(
                        stagiaire.Email,
                        "Vos informations de connexion",
                        $"Bonjour {stagiaire.Prenom},\n\n" +
                        $"Votre compte a été créé avec succès.\n\n" +
                        $"Nom d'utilisateur: {stagiaire.Username}\n" +
                        $"Mot de passe: {generatedPassword}\n\n" +
                        $"Veuillez changer votre mot de passe après votre première connexion.\n\n" +
                        $"Cordialement,\nLe service des stages"
                    );

                    // Si l'email est envoyé avec succès, valider la transaction
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Si l'envoi d'email échoue, annuler la transaction
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        $"Échec lors de l'envoi de l'email: {ex.Message}");
                }

                return CreatedAtAction(nameof(GetStagiaire), new { id = stagiaire.Id }, stagiaire.ToDto());
            }
            catch (Exception ex)
            {
                // Si une exception se produit lors de la création du compte, annuler la transaction
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Échec lors de la création du compte: {ex.Message}");
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
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Service des Stages", _configuration["Email:From"]));
            email.To.Add(new MailboxAddress("", destinataire));
            email.Subject = sujet;
            email.Body = new TextPart(TextFormat.Plain) { Text = corps };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await client.ConnectAsync(_configuration["Email:Host"], int.Parse(_configuration["Email:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Remonter l'exception pour annuler la transaction
                throw new Exception($"Échec de l'envoi d'email: {ex.Message}", ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "MembreDirection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StagiaireDto>> UpdateStagiaire([FromRoute] int id, [FromBody] StagiaireUpdateDto stagiaireDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            if (!string.IsNullOrEmpty(stagiaireDto.Email) &&
                stagiaireDto.Email != stagiaire.Email &&
                await _db.Stagiaires.AnyAsync(s => s.Email == stagiaireDto.Email))
            {
                return BadRequest($"Un stagiaire avec l'email {stagiaireDto.Email} existe déjà.");
            }

            if (!string.IsNullOrEmpty(stagiaireDto.Nom))
                stagiaire.Nom = stagiaireDto.Nom;

            if (!string.IsNullOrEmpty(stagiaireDto.Prenom))
                stagiaire.Prenom = stagiaireDto.Prenom;

            if (!string.IsNullOrEmpty(stagiaireDto.Email))
                stagiaire.Email = stagiaireDto.Email;

            if (!string.IsNullOrEmpty(stagiaireDto.Telephone))
                stagiaire.Telephone = stagiaireDto.Telephone;

            if (!string.IsNullOrEmpty(stagiaireDto.Universite))
                stagiaire.Universite = stagiaireDto.Universite;

            if (!string.IsNullOrEmpty(stagiaireDto.Specialite))
                stagiaire.Specialite = stagiaireDto.Specialite;

            stagiaire.Status = stagiaireDto.Status;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(stagiaire.ToDto());
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Une erreur s'est produite lors de la mise à jour du stagiaire: {ex.Message}");
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "MembreDirection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StagiaireDto>> UpdateStagiaireStatus([FromRoute] int id, [FromBody] UpdateStagiaireStatusDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            stagiaire.Status = updateDto.Status;

            await _db.SaveChangesAsync();
            return Ok(stagiaire.ToDto());
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "MembreDirection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StagiaireDto>> UpdateStagiairePartiel([FromRoute] int id, [FromBody] JsonPatchDocument<Stagiaire> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Le document patch est null.");
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            patchDoc.ApplyTo(stagiaire, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.SaveChangesAsync();
                return Ok(stagiaire.ToDto());
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Une erreur s'est produite lors de la mise à jour partielle du stagiaire: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MembreDirection")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteStagiaire([FromRoute] int id)
        {
            var stagiaire = await _db.Stagiaires
                .Include(s => s.DemandeDeStage)
                .Include(s => s.Stage)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            if (stagiaire.Stage != null &&
                (stagiaire.Stage.Statut == StatutStage.EnAttente ||
                 stagiaire.Stage.Statut == StatutStage.EnCours ||
                 stagiaire.Stage.Statut == StatutStage.Prolonge))
            {
                return BadRequest("Impossible de supprimer un stagiaire avec un stage actif.");
            }

            try
            {
                _db.Stagiaires.Remove(stagiaire);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Une erreur s'est produite lors de la suppression du stagiaire: {ex.Message}");
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> SearchStagiaires(
            [FromQuery] string nom = null,
            [FromQuery] string prenom = null,
            [FromQuery] string universite = null,
            [FromQuery] string specialite = null,
            [FromQuery] StagiaireStatus? status = null)
        {
            var query = _db.Stagiaires.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nom))
                query = query.Where(s => s.Nom.Contains(nom));

            if (!string.IsNullOrWhiteSpace(prenom))
                query = query.Where(s => s.Prenom.Contains(prenom));

            if (!string.IsNullOrWhiteSpace(universite))
                query = query.Where(s => s.Universite.Contains(universite));

            if (!string.IsNullOrWhiteSpace(specialite))
                query = query.Where(s => s.Specialite.Contains(specialite));

            if (status.HasValue)
                query = query.Where(s => s.Status == status.Value);

            var stagiaires = await query.ToListAsync();
            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> GetStagiairesByStatus(StagiaireStatus status)
        {
            var stagiaires = await _db.Stagiaires
                .Where(s => s.Status == status)
                .ToListAsync();

            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        [HttpGet("encadreur/{encadreurId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> GetStagiairesByEncadreur(int encadreurId)
        {
            if (!await _db.Encadreurs.AnyAsync(e => e.Id == encadreurId))
            {
                return NotFound($"Encadreur avec l'ID {encadreurId} non trouvé.");
            }

            var stagiaires = await _db.Stagiaires
                .Where(s => s.Stage != null && s.Stage.EncadreurId == encadreurId)
                .ToListAsync();

            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        [HttpGet("current")]
        [Authorize(Roles = "Stagiaire")]
        public async Task<ActionResult<StagiaireDto>> GetCurrentStagiaire()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var stagiaire = await _db.Stagiaires.FindAsync(id);
            if (stagiaire == null)
                return NotFound("Stagiaire non trouvé");

            return stagiaire.ToDto();
        }

        [HttpPut("update-password")]
        [Authorize(Roles = "Stagiaire")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var stagiaire = await _db.Stagiaires.FindAsync(id);
            if (stagiaire == null)
                return NotFound("Stagiaire non trouvé");

            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Stagiaire>();
            var verificationResult = passwordHasher.VerifyHashedPassword(stagiaire, stagiaire.MotDePasse, model.CurrentPassword);
            if (verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                return BadRequest("Le mot de passe actuel est incorrect");

            stagiaire.MotDePasse = passwordHasher.HashPassword(stagiaire, model.NewPassword);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Mot de passe mis à jour avec succès" });
        }

        [HttpPut("update-stagiaire-info")]
        [Authorize(Roles = "Stagiaire")]
        public async Task<IActionResult> UpdateStagiaireInfo([FromBody] UpdateStagiaireInfoDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var stagiaire = await _db.Stagiaires.FindAsync(id);
            if (stagiaire == null)
                return NotFound("Stagiaire non trouvé");

            if (!string.IsNullOrEmpty(updateDto.Email))
                stagiaire.Email = updateDto.Email;

            if (!string.IsNullOrEmpty(updateDto.Telephone))
                stagiaire.Telephone = updateDto.Telephone;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(new { message = "Informations mises à jour avec succès" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Stagiaires.Any(s => s.Id == id))
                    return NotFound("Stagiaire non trouvé");
                else
                    throw;
            }
        }

        [Authorize]
        [HttpGet("authenticated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AuthenticatedEndpoint()
        {
            return Ok("Vous êtes authentifié !");
        }
    }
}