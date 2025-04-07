using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.EncadreurDTO;
using StageManager.Mapping;
using StageManager.Models;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/Encadreurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EncadreurDto>>> GetEncadreurs()
        {
            var encadreurs = await _context.Encadreurs
                .Include(e => e.Departement)
                .Include(e => e.Stages)
                .ToListAsync();

            return encadreurs.Select(e => EncadreurMapping.ToDto(e)).ToList();
        }

        // GET: api/Encadreurs/5
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

            return EncadreurMapping.ToDto(encadreur);
        }

        // POST: api/Encadreurs
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

            // Utiliser le mapping pour convertir DTO en entité
            var encadreur = EncadreurMapping.ToEntity(dto);

            // Hasher le mot de passe (dans un environnement réel)
            // encadreur.MotDePasse = HashPassword(dto.MotDePasse);

            _context.Encadreurs.Add(encadreur);
            await _context.SaveChangesAsync();

            // Recharger l'encadreur avec son département pour le DTO de retour
            encadreur = await _context.Encadreurs
                .Include(e => e.Departement)
                .FirstOrDefaultAsync(e => e.Id == encadreur.Id);

            return CreatedAtAction(
                nameof(GetEncadreur),
                new { id = encadreur.Id },
                EncadreurMapping.ToDto(encadreur)
            );
        }

        // PUT: api/Encadreurs/5
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

        // DELETE: api/Encadreurs/5
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