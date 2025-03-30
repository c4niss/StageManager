using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DepartementDTO;
using StageManager.DTO.DepartementDTO.StageManager.DTOs;
using StageManager.Models;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartementsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Departement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartementDto>>> GetDepartements()
        {
            var departements = await _context.Departements
                .Include(d => d.ChefDepartement)
                .Include(d => d.Encadreurs)
                .ToListAsync();

            return departements.Select(d => new DepartementDto
            {
                Id = d.Id,
                Nom = d.Nom,
                ChefDepartementId = d.ChefDepartementId,
                ChefDepartementNom = d.ChefDepartement != null ? $"{d.ChefDepartement.Nom} {d.ChefDepartement.Prenom}" : null,
                NombreEncadreurs = d.Encadreurs?.Count ?? 0,
                // Pour calculer le nombre de stagiaires actuels, vous auriez besoin d'une requête plus complexe
                NombreStagiairesActuels = 0 // Implémentation à compléter
            }).ToList();
        }

        // GET: api/Departement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartementDto>> GetDepartement(int id)
        {
            var departement = await _context.Departements
                .Include(d => d.ChefDepartement)
                .Include(d => d.Encadreurs)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (departement == null)
            {
                return NotFound();
            }

            // Calcul du nombre de stagiaires actuels
            var nombreStagiaires = await _context.Stages
                .Where(s => s.DepartementId == id && s.Statut == StatutStage.EnCours)
                .SelectMany(s => s.Stagiaires)
                .CountAsync();

            return new DepartementDto
            {
                Id = departement.Id,
                Nom = departement.Nom,
                ChefDepartementId = departement.ChefDepartementId,
                ChefDepartementNom = departement.ChefDepartement != null ? $"{departement.ChefDepartement.Nom} {departement.ChefDepartement.Prenom}" : null,
                NombreEncadreurs = departement.Encadreurs?.Count ?? 0,
                NombreStagiairesActuels = nombreStagiaires
            };
        }

        // POST: api/Departement
        [HttpPost]
        public async Task<ActionResult<DepartementDto>> CreateDepartement(CreateDepartementDto dto)
        {
            // Vérifier si le chef existe (s'il a été fourni)
            if (dto.ChefDepartementId.HasValue)
            {
                var chefExists = await _context.ChefDepartements.AnyAsync(c => c.Id == dto.ChefDepartementId);
                if (!chefExists)
                {
                    return BadRequest("Chef de département non trouvé");
                }
            }

            var departement = new Departement
            {
                Nom = dto.Nom,
                ChefDepartementId = dto.ChefDepartementId
            };

            _context.Departements.Add(departement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartement), new { id = departement.Id }, new DepartementDto
            {
                Id = departement.Id,
                Nom = departement.Nom,
                ChefDepartementId = departement.ChefDepartementId,
                NombreEncadreurs = 0,
                NombreStagiairesActuels = 0
            });
        }

        // PUT: api/Departement/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartement(int id, UpdateDepartementDto dto)
        {
            var departement = await _context.Departements.FindAsync(id);
            if (departement == null)
            {
                return NotFound();
            }

            // Mise à jour des propriétés si elles sont fournies
            if (!string.IsNullOrEmpty(dto.Nom))
            {
                departement.Nom = dto.Nom;
            }

            if (dto.ChefDepartementId.HasValue)
            {
                var chefExists = await _context.ChefDepartements.AnyAsync(c => c.Id == dto.ChefDepartementId);
                if (!chefExists)
                {
                    return BadRequest("Chef de département non trouvé");
                }
                departement.ChefDepartementId = dto.ChefDepartementId;
            }

            _context.Departements.Update(departement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Departement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartement(int id)
        {
            var departement = await _context.Departements
                .Include(d => d.Encadreurs)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (departement == null)
            {
                return NotFound();
            }

            // Vérifier si des encadreurs sont associés
            if (departement.Encadreurs != null && departement.Encadreurs.Any())
            {
                return BadRequest("Impossible de supprimer: des encadreurs sont associés à ce département");
            }

            _context.Departements.Remove(departement);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}