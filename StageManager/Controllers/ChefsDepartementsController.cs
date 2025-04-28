using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ChefDepartementDTO;
using StageManager.Mapping;
using StageManager.Models;
using Microsoft.AspNetCore.Identity;
using TestRestApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefDepartementsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChefDepartementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ChefDepartements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChefDepartementDto>>> GetChefDepartements()
        {
            var chefs = await _context.ChefDepartements
                .Include(c => c.Departement)
                .ToListAsync();

            return Ok(chefs.Select(c => c.ToDto()));
        }

        // GET: api/ChefDepartements/5
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

        // POST: api/ChefDepartements
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ChefDepartementDto>> CreateChefDepartement(CreateChefDepartementDto createDto)
        {
            // Validation explicite que DepartementId existe et est valide
            if (createDto.DepartementId <= 0)
            {
                return BadRequest("Un département valide est requis pour créer un chef de département");
            }

            var departement = await _context.Departements.FindAsync(createDto.DepartementId);
            if (departement == null)
            {
                return BadRequest("Département non trouvé");
            }

            // Le reste du code reste inchangé
            var passwordHasher = new PasswordHasher<ChefDepartement>();
            string hashedPassword = passwordHasher.HashPassword(null, createDto.MotDePasse);

            var chefDepartement = new ChefDepartement
            {
                Nom = createDto.Nom,
                Prenom = createDto.Prenom,
                Username = createDto.Username,
                Email = createDto.Email,
                Telephone = createDto.Telephone,
                MotDePasse = hashedPassword,
                DepartementId = createDto.DepartementId,
                EstActif = false,
                Role = UserRoles.ChefDepartement
            };

            _context.ChefDepartements.Add(chefDepartement);
            await _context.SaveChangesAsync();

            // Utiliser une requête SQL directe pour mettre à jour explicitement le DepartementId
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Utilisateurs SET DepartementId = {0} WHERE Id = {1} AND TypeUtilisateur = 'ChefDepartement'",
                createDto.DepartementId, chefDepartement.Id);

            // Mettre à jour le département
            departement.ChefDepartementId = chefDepartement.Id;
            await _context.SaveChangesAsync();

            // Recharger l'entité pour s'assurer que nous avons les dernières données
            await _context.Entry(chefDepartement).ReloadAsync();

            return CreatedAtAction(nameof(GetChefDepartement), new { id = chefDepartement.Id }, chefDepartement.ToDto());
        }


        // PUT: api/ChefDepartements/5
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

        // DELETE: api/ChefDepartements/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteChefDepartement(int id)
        {
            var chef = await _context.ChefDepartements.FindAsync(id);
            if (chef == null)
            {
                return NotFound();
            }

            // Trouver tous les départements qui ont ce chef comme ChefDepartementId
            var departements = await _context.Departements
                .Where(d => d.ChefDepartementId == id)
                .ToListAsync();

            // Réinitialiser le ChefDepartementId pour ces départements
            foreach (var departement in departements)
            {
                departement.ChefDepartementId = null;
            }

            _context.ChefDepartements.Remove(chef);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/ChefDepartements/current
        [HttpGet("current")]
        [Authorize(Roles = "ChefDepartement")]
        public async Task<ActionResult<ChefDepartementDto>> GetCurrentChefDepartement()
        {
            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
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

        // PUT: api/ChefDepartements/update-password
        [HttpPut("update-password")]
        [Authorize(Roles = "ChefDepartement")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var chefDepartement = await _context.ChefDepartements.FindAsync(id);
            if (chefDepartement == null)
                return NotFound("Chef de département non trouvé");

            // Vérifier que le mot de passe actuel est correct
            var passwordHasher = new PasswordHasher<ChefDepartement>();
            var verificationResult = passwordHasher.VerifyHashedPassword(chefDepartement, chefDepartement.MotDePasse, model.CurrentPassword);
            if (verificationResult == PasswordVerificationResult.Failed)
                return BadRequest("Le mot de passe actuel est incorrect");

            // Mettre à jour le mot de passe
            chefDepartement.MotDePasse = passwordHasher.HashPassword(chefDepartement, model.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mot de passe mis à jour avec succès" });
        }
        // PUT: api/ChefDepartements/update-profile
        [HttpPut("update-profile")]
        [Authorize(Roles = "ChefDepartement")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var chefDepartement = await _context.ChefDepartements.FindAsync(id);
            if (chefDepartement == null)
                return NotFound("Chef de département non trouvé");

            // Mettre à jour uniquement les champs autorisés (email et téléphone)
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