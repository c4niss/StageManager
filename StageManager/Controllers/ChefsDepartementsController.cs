using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ChefDepartementDTO;
using StageManager.Mapping;
using StageManager.Models;
using Microsoft.AspNetCore.Identity;
using TestRestApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Transactions;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefDepartementsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChefDepartementsController> _logger;

        public ChefDepartementsController(AppDbContext context, IConfiguration configuration, ILogger<ChefDepartementsController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChefDepartementDto>>> GetChefDepartements()
        {
            var chefs = await _context.ChefDepartements
                .Include(c => c.Departement)
                .ToListAsync();

            return Ok(chefs.Select(c => c.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChefDepartementDto>> GetChefDepartement(int id)
        {
            var chef = await _context.ChefDepartements
                .Include(c => c.Departement)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chef == null)
            {
                return NotFound();
            }

            return Ok(chef.ToDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ChefDepartementDto>> CreateChefDepartement(CreateChefDepartementDto createDto)
        {
            if (createDto.DepartementId <= 0)
            {
                return BadRequest("Un département valide est requis pour créer un chef de département");
            }

            var departement = await _context.Departements.FindAsync(createDto.DepartementId);
            if (departement == null)
            {
                return BadRequest("Département non trouvé");
            }

            string generatedPassword = GenerateRandomPassword(8);
            var passwordHasher = new PasswordHasher<ChefDepartement>();
            string hashedPassword = passwordHasher.HashPassword(null, generatedPassword);

            var chefDepartement = new ChefDepartement
            {
                Nom = createDto.Nom,
                Prenom = createDto.Prenom,
                Username = createDto.Username,
                Email = createDto.Email,
                Telephone = createDto.Telephone,
                MotDePasse = hashedPassword,
                DepartementId = createDto.DepartementId,
                EstActif = true,
                Role = UserRoles.ChefDepartement
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.ChefDepartements.Add(chefDepartement);
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Utilisateurs SET DepartementId = {0} WHERE Id = {1} AND TypeUtilisateur = 'ChefDepartement'",
                        createDto.DepartementId, chefDepartement.Id);

                    departement.ChefDepartementId = chefDepartement.Id;
                    await _context.SaveChangesAsync();

                    // Envoyer l'email avant de valider la transaction
                    await EnvoyerEmailAsync(
                        chefDepartement.Email,
                        "Vos informations de connexion",
                        $"Bonjour {chefDepartement.Prenom},\n\n" +
                        $"Votre compte a été créé avec succès.\n\n" +
                        $"Nom d'utilisateur: {chefDepartement.Username}\n" +
                        $"Mot de passe: {generatedPassword}\n\n" +
                        $"Veuillez changer votre mot de passe après votre première connexion.\n\n" +
                        $"Cordialement,\nLe service des stages"
                    );

                    // Si l'envoi d'email réussit, on valide la transaction
                    await transaction.CommitAsync();
                    _logger.LogInformation("Chef de département créé avec succès: {Id}, Email envoyé à: {Email}", chefDepartement.Id, chefDepartement.Email);

                    await _context.Entry(chefDepartement).ReloadAsync();
                    return CreatedAtAction(nameof(GetChefDepartement), new { id = chefDepartement.Id }, chefDepartement.ToDto());
                }
                catch (Exception ex)
                {
                    // En cas d'erreur (y compris lors de l'envoi d'email), on annule la transaction
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Erreur lors de la création du chef de département ou de l'envoi d'email: {Message}", ex.Message);
                    return StatusCode(500, "Une erreur est survenue lors de la création du compte ou de l'envoi de l'email: " + ex.Message);
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
                _logger.LogInformation("Email envoyé avec succès à: {Email}", destinataire);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'envoi de l'email à {Email}: {Message}", destinataire, ex.Message);
                throw new Exception($"Échec de l'envoi de l'email: {ex.Message}", ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateChefDepartement(int id, UpdateChefDepartementDto updateDto)
        {
            var chef = await _context.ChefDepartements.FindAsync(id);
            if (chef == null)
            {
                return NotFound();
            }
            chef.Nom = updateDto.Nom;
            chef.Prenom = updateDto.Prenom;
            chef.Email = updateDto.Email;
            chef.Telephone = updateDto.Telephone;

            if (updateDto.DepartementId.HasValue)
            {
                var departement = await _context.Departements.FindAsync(updateDto.DepartementId.Value);
                if (departement == null)
                    return BadRequest("Département non trouvé");
                chef.DepartementId = updateDto.DepartementId.Value;
            }

            if (updateDto.EstActif.HasValue)
                chef.EstActif = updateDto.EstActif.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChefDepartementExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteChefDepartement(int id)
        {
            var chef = await _context.ChefDepartements.FindAsync(id);
            if (chef == null)
            {
                return NotFound();
            }

            var departements = await _context.Departements
                .Where(d => d.ChefDepartementId == id)
                .ToListAsync();

            foreach (var departement in departements)
            {
                departement.ChefDepartementId = null;
            }

            _context.ChefDepartements.Remove(chef);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("current")]
        [Authorize(Roles = "ChefDepartement")]
        public async Task<ActionResult<ChefDepartementDto>> GetCurrentChefDepartement()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var chefDepartement = await _context.ChefDepartements
                .Include(c => c.Departement)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chefDepartement == null)
                return NotFound("Chef de département non trouvé");

            return chefDepartement.ToDto();
        }

        [HttpPut("update-password")]
        [Authorize(Roles = "ChefDepartement")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var chefDepartement = await _context.ChefDepartements.FindAsync(id);
            if (chefDepartement == null)
                return NotFound("Chef de département non trouvé");

            var passwordHasher = new PasswordHasher<ChefDepartement>();
            var verificationResult = passwordHasher.VerifyHashedPassword(chefDepartement, chefDepartement.MotDePasse, model.CurrentPassword);
            if (verificationResult == PasswordVerificationResult.Failed)
                return BadRequest("Le mot de passe actuel est incorrect");

            chefDepartement.MotDePasse = passwordHasher.HashPassword(chefDepartement, model.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mot de passe mis à jour avec succès" });
        }

        [HttpPut("update-profile")]
        [Authorize(Roles = "ChefDepartement")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var chefDepartement = await _context.ChefDepartements.FindAsync(id);
            if (chefDepartement == null)
                return NotFound("Chef de département non trouvé");

            if (updateDto.Email != null)
                chefDepartement.Email = updateDto.Email;
            if (updateDto.Telephone != null)
                chefDepartement.Telephone = updateDto.Telephone;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Profil mis à jour avec succès" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la mise à jour: {ex.Message}");
            }
        }

        private bool ChefDepartementExists(int id)
        {
            return _context.ChefDepartements.Any(e => e.Id == id);
        }
    }
}