using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.Models;
using System.Security.Claims;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<Utilisateur> _passwordHasher;

        public AdminController(AppDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<Utilisateur>();
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _db.Utilisateurs.ToListAsync();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _db.Utilisateurs.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var user = await _db.Utilisateurs.FindAsync(id);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            return Ok(user);
        }

        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var user = await _db.Utilisateurs.FindAsync(id);
            if (user == null)
                return NotFound();

            user.EstActif = !user.EstActif;
            await _db.SaveChangesAsync();

            return Ok(user);
        }
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var user = await _db.Utilisateurs.FindAsync(id);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            // Vérifier que le mot de passe actuel est correct
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.MotDePasse, model.CurrentPassword);
            if (verificationResult == PasswordVerificationResult.Failed)
                return BadRequest("Le mot de passe actuel est incorrect");

            // Mettre à jour le mot de passe
            user.MotDePasse = _passwordHasher.HashPassword(user, model.NewPassword);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Mot de passe mis à jour avec succès" });
        }
        [HttpPut("update-info")]
        public async Task<IActionResult> UpdateAdminInfo([FromBody] UpdateAdminInfoModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var admin = await _db.Utilisateurs.FindAsync(id);
            if (admin == null)
                return NotFound("Utilisateur non trouvé");

            // Mettre à jour uniquement les propriétés fournies
            if (!string.IsNullOrEmpty(model.Nom))
                admin.Nom = model.Nom;

            if (!string.IsNullOrEmpty(model.Prenom))
                admin.Prenom = model.Prenom;

            if (!string.IsNullOrEmpty(model.Email))
                admin.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Username))
                admin.Username = model.Username;

            if (!string.IsNullOrEmpty(model.Telephone))
                admin.Telephone = model.Telephone;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Informations mises à jour avec succès", admin });
        }
    }
}