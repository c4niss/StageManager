using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.EncadreurDTO;
using StageManager.Mapping;
using StageManager.Models;
using System.Security.Cryptography;
using System.Text;
using TestRestApi.Data;

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

            var encadreur = new Encadreur
            {
                Nom = createDto.Nom,
                Prenom = createDto.Prenom,
                Username = createDto.Username,
                Email = createDto.Email,
                Telephone = createDto.Telephone,
                MotDePasse = HashPassword(createDto.MotDePasse),
                DepartementId = createDto.DepartementId,
                DomaineId = createDto.DomaineId,
                EstDisponible = true,
                NbrStagiaires = 0,
                StagiaireMax = 3,
                Role = "Encadreur"
            };

            _context.Encadreurs.Add(encadreur);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEncadreur), new { id = encadreur.Id }, encadreur.ToDto());
        }

        // PUT: api/Encadreurs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEncadreur(int id, UpdateEncadreurDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("L'ID ne correspond pas");
            }

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

        private bool EncadreurExists(int id)
        {
            return _context.Encadreurs.Any(e => e.Id == id);
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}