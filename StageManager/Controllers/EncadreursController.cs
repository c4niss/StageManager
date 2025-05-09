using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.EncadreurDTO;
using StageManager.Mapping;
using StageManager.Models;
using Microsoft.AspNetCore.Identity;
using TestRestApi.Data;
using System.Security.Claims;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncadreursController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EncadreursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Encadreurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EncadreurDto>>> GetEncadreurs()
        {
            var encadreurs = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Domaine)
                .ToListAsync();

            return Ok(encadreurs.Select(e => e.ToDto()));
        }

        // GET: api/Encadreurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EncadreurDto>> GetEncadreur(int id)
        {
            var encadreur = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Domaine)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encadreur == null)
            {
                return NotFound();
            }

            return Ok(encadreur.ToDto());
        }
        // POST: api/Encadreurs
        [HttpPost]
        public async Task<ActionResult<EncadreurDto>> CreateEncadreur(CreateEncadreurDto createDto)
        {
            // Validate the departement exists
            var departement = await _context.Departements.FindAsync(createDto.DepartementId);
            if (departement == null)
            {
                return BadRequest("Département non trouvé");
            }

            // Validate the domaine exists and belongs to the specified departement
            var domaine = await _context.Domaines
                .Include(d => d.Departement)
                .FirstOrDefaultAsync(d => d.Id == createDto.DomaineId);

            if (domaine == null)
            {
                return BadRequest("Domaine non trouvé");
            }

            // Check if the domaine belongs to the specified departement
            if (domaine.DepartementId != createDto.DepartementId)
            {
                return BadRequest("Le domaine sélectionné n'appartient pas au département spécifié");
            }

            // Vérifier si le nom d'utilisateur existe déjà
            var existingUsername = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Username == createDto.Username);
            if (existingUsername != null)
            {
                return BadRequest("Le nom d'utilisateur existe déjà.");
            }

            // Utiliser le même hashage de mot de passe que StagiairesController
            var passwordHasher = new PasswordHasher<Encadreur>();

            var encadreur = new Encadreur
            {
                Nom = createDto.Nom,
                Prenom = createDto.Prenom,
                Username = createDto.Username,
                Email = createDto.Email,
                Telephone = createDto.Telephone,
                MotDePasse = passwordHasher.HashPassword(null, createDto.MotDePasse),
                Fonction = createDto.Fonction,
                DepartementId = createDto.DepartementId,
                DomaineId = createDto.DomaineId,
                EstDisponible = true,
                EstActif = false,
                NbrStagiaires = 0,
                StagiaireMax = 3,
                Role = UserRoles.Encadreur
            };

            _context.Encadreurs.Add(encadreur);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEncadreur), new { id = encadreur.Id }, encadreur.ToDto());
        }

        // PUT: api/Encadreurs/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEncadreur(int id, UpdateEncadreurDto updateDto)
        {
            var encadreur = await _context.Encadreurs.FindAsync(id);
            if (encadreur == null)
            {
                return NotFound();
            }

            // Update properties if provided
            if (!string.IsNullOrEmpty(updateDto.Nom))
                encadreur.Nom = updateDto.Nom;

            if (!string.IsNullOrEmpty(updateDto.Prenom))
                encadreur.Prenom = updateDto.Prenom;

            if (!string.IsNullOrEmpty(updateDto.Email))
                encadreur.Email = updateDto.Email;

            if (!string.IsNullOrEmpty(updateDto.Telephone))
                encadreur.Telephone = updateDto.Telephone;

            if (!string.IsNullOrEmpty(updateDto.Username))
                encadreur.Username = updateDto.Username;

            if (!string.IsNullOrEmpty(updateDto.Fonction))
                encadreur.Fonction = updateDto.Fonction;

            if (updateDto.EstDisponible.HasValue)
                encadreur.EstDisponible = updateDto.EstDisponible.Value;

            if (updateDto.StagiaireMax.HasValue)
                encadreur.StagiaireMax = updateDto.StagiaireMax.Value;
            if (updateDto.EstActif.HasValue)
                encadreur.EstActif = updateDto.EstActif.Value;
            if (updateDto.DepartementId.HasValue)
            {
                var departement = await _context.Departements.FindAsync(updateDto.DepartementId.Value);
                if (departement == null)
                    return BadRequest("Département non trouvé");
                encadreur.DepartementId = updateDto.DepartementId.Value;
            }

            if (updateDto.DomaineId.HasValue)
            {
                var domaine = await _context.Domaines.FindAsync(updateDto.DomaineId.Value);
                if (domaine == null)
                    return BadRequest("Domaine non trouvé");
                encadreur.DomaineId = updateDto.DomaineId.Value;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EncadreurExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Encadreurs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEncadreur(int id)
        {
            var encadreur = await _context.Encadreurs.FindAsync(id);
            if (encadreur == null)
            {
                return NotFound();
            }

            _context.Encadreurs.Remove(encadreur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Encadreurs/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<EncadreurDto>>> GetEncadreursDisponibles()
        {
            var encadreurs = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Domaine)
                .Where(e => e.EstDisponible && e.NbrStagiaires < e.StagiaireMax)
                .ToListAsync();

            return Ok(encadreurs.Select(e => e.ToDto()));
        }

        // GET: api/Encadreurs/byDepartement/5
        [HttpGet("byDepartement/{departementId}")]
        public async Task<ActionResult<IEnumerable<EncadreurDto>>> GetEncadreursByDepartement(int departementId)
        {
            var encadreurs = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Domaine)
                .Where(e => e.DepartementId == departementId)
                .ToListAsync();

            return Ok(encadreurs.Select(e => e.ToDto()));
        }

        // GET: api/Encadreurs/byDomaine/5
        [HttpGet("byDomaine/{domaineId}")]
        public async Task<ActionResult<IEnumerable<EncadreurDto>>> GetEncadreursByDomaine(int domaineId)
        {
            var encadreurs = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Domaine)
                .Where(e => e.DomaineId == domaineId)
                .ToListAsync();

            return Ok(encadreurs.Select(e => e.ToDto()));
        }
        // GET: api/Encadreurs/current
        [HttpGet("current")]
        [Authorize(Roles = "Encadreur")]
        public async Task<ActionResult<EncadreurDto>> GetCurrentEncadreur()
        {
            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var encadreur = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Domaine)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encadreur == null)
                return NotFound("Encadreur non trouvé");

            return encadreur.ToDto();
        }

        // PUT: api/Encadreurs/update-password
        [HttpPut("update-password")]
        [Authorize(Roles = "Encadreur")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var encadreur = await _context.Encadreurs.FindAsync(id);
            if (encadreur == null)
                return NotFound("Encadreur non trouvé");

            // Vérifier que le mot de passe actuel est correct
            var passwordHasher = new PasswordHasher<Encadreur>();
            var verificationResult = passwordHasher.VerifyHashedPassword(encadreur, encadreur.MotDePasse, model.CurrentPassword);
            if (verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                return BadRequest("Le mot de passe actuel est incorrect");

            // Mettre à jour le mot de passe
            encadreur.MotDePasse = passwordHasher.HashPassword(encadreur, model.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mot de passe mis à jour avec succès" });
        }
        // PUT: api/Encadreurs/update-encadreur-info
        [HttpPut("update-encadreur-info")]
        [Authorize(Roles = "Encadreur")]
        public async Task<IActionResult> UpdateEncadreurInfo([FromBody] UpdateEncadreurInfoDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Récupérer l'ID de l'utilisateur connecté depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized("Token invalide ou utilisateur non identifié");

            var encadreur = await _context.Encadreurs.FindAsync(id);
            if (encadreur == null)
                return NotFound("Encadreur non trouvé");

            // Mettre à jour les propriétés si elles sont fournies
            if (!string.IsNullOrEmpty(updateDto.Email))
                encadreur.Email = updateDto.Email;

            if (!string.IsNullOrEmpty(updateDto.Telephone))
                encadreur.Telephone = updateDto.Telephone;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Informations mises à jour avec succès" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EncadreurExists(id))
                    return NotFound("Encadreur non trouvé");
                else
                    throw;
            }
        }

        private bool EncadreurExists(int id)
        {
            return _context.Encadreurs.Any(e => e.Id == id);
        }
    }
}