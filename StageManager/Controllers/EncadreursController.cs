
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.EncadreurDTO;
using StageManager.Models;
using System.Threading.Tasks;
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

        // GET: api/Encadreur
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EncadreurDto>>> GetEncadreurs()
        {
            var encadreurs = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Stages)
                .ToListAsync();

            return encadreurs.Select(e => new EncadreurDto
            {
                Id = e.Id,
                Nom = e.Nom,
                Prenom = e.Prenom,
                Email = e.Email,
                Telephone = e.Telephone,
                Fonction = e.Fonction,
                EstDisponible = e.EstDisponible,
                NbrStagiaires = e.NbrStagiaires,
                StagiaireMax = e.StagiaireMax,
                DepartementId = e.DepartementId,
                DepartementNom = e.Departement?.Nom
            }).ToList();
        }

        // GET: api/Encadreur/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EncadreurDto>> GetEncadreur(int id)
        {
            var encadreur = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Stages)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encadreur == null)
            {
                return NotFound();
            }

            return new EncadreurDto
            {
                Id = encadreur.Id,
                Nom = encadreur.Nom,
                Prenom = encadreur.Prenom,
                Email = encadreur.Email,
                Telephone = encadreur.Telephone,
                Fonction = encadreur.Fonction,
                EstDisponible = encadreur.EstDisponible,
                NbrStagiaires = encadreur.NbrStagiaires,
                StagiaireMax = encadreur.StagiaireMax,
                DepartementId = encadreur.DepartementId,
                DepartementNom = encadreur.Departement?.Nom
            };
        }

        // POST: api/Encadreur
        [HttpPost]
        public async Task<ActionResult<EncadreurDto>> CreateEncadreur(CreateEncadreurDto dto)
        {
            // Vérifier si le département existe
            if (dto.DepartementId > 0)
            {
                var departement = await _context.Departements.FindAsync(dto.DepartementId);
                if (departement == null)
                {
                    return BadRequest("Département non trouvé");
                }
            }

            // Vérifier si l'email est déjà utilisé
            var emailExists = await _context.Encadreurs.AnyAsync(e => e.Email == dto.Email);
            if (emailExists)
            {
                return BadRequest("Un compte avec cet email existe déjà");
            }

            var encadreur = new Encadreur
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Telephone = dto.Telephone,
                MotDePasse = dto.MotDePasse, // À remplacer par un hash dans le cas réel
                DepartementId = dto.DepartementId,
                Role = "Encadreur",
                EstActif = true,
                Fonction = "Encadreur", // Valeur par défaut
                EstDisponible = true,
                NbrStagiaires = 0,
                StagiaireMax = 3 // Valeur par défaut
            };

            _context.Encadreurs.Add(encadreur);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEncadreur),
                new { id = encadreur.Id },
                new EncadreurDto
                {
                    Id = encadreur.Id,
                    Nom = encadreur.Nom,
                    Prenom = encadreur.Prenom,
                    Email = encadreur.Email,
                    Telephone = encadreur.Telephone,
                    Fonction = encadreur.Fonction,
                    EstDisponible = encadreur.EstDisponible,
                    NbrStagiaires = encadreur.NbrStagiaires,
                    StagiaireMax = encadreur.StagiaireMax,
                    DepartementId = encadreur.DepartementId
                });
        }

        // PUT: api/Encadreur/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEncadreur(int id, [FromBody] EncadreurDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("L'ID de l'URL ne correspond pas à l'ID dans le corps de la requête");
            }

            var encadreur = await _context.Encadreurs.FindAsync(id);
            if (encadreur == null)
            {
                return NotFound();
            }

            // Vérifier si le département existe
            if (dto.DepartementId.HasValue)
            {
                var departement = await _context.Departements.FindAsync(dto.DepartementId);
                if (departement == null)
                {
                    return BadRequest("Département non trouvé");
                }
            }

            // Mettre à jour les propriétés
            encadreur.Nom = dto.Nom;
            encadreur.Prenom = dto.Prenom;
            encadreur.Email = dto.Email;
            encadreur.Telephone = dto.Telephone;
            encadreur.Fonction = dto.Fonction;
            encadreur.EstDisponible = dto.EstDisponible;
            encadreur.StagiaireMax = dto.StagiaireMax;
            encadreur.DepartementId = dto.DepartementId;

            _context.Entry(encadreur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EncadreurExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Encadreur/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEncadreur(int id)
        {
            var encadreur = await _context.Encadreurs
                .Include(e => e.Stages)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encadreur == null)
            {
                return NotFound();
            }

            // Vérifier si l'encadreur a des stages associés
            if (encadreur.Stages != null && encadreur.Stages.Any())
            {
                return BadRequest("Impossible de supprimer cet encadreur car il a des stages associés");
            }

            _context.Encadreurs.Remove(encadreur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EncadreurExists(int id)
        {
            return _context.Encadreurs.Any(e => e.Id == id);
        }
    }
}