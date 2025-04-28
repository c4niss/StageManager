using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DepartementDTO;
using StageManager.DTO.DepartementDTO.StageManager.DTOs;
using StageManager.Mapping;
using StageManager.Models;
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

        // GET: api/Departements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartementDto>>> GetDepartements()
        {
            var departements = await _context.Departements
                .Include(d => d.ChefDepartement)
                .Include(d => d.Encadreurs)
                .ToListAsync();

            return Ok(departements.Select(d => d.ToDto()));
        }

        // GET: api/Departements/5
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

            return Ok(departement.ToDto());
        }

        // POST: api/Departements
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DepartementDto>> CreateDepartement(CreateDepartementDto createDto)
        {
            var departement = new Departement
            {
                Nom = createDto.Nom,
            };
            _context.Departements.Add(departement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartement), new { id = departement.Id }, departement.ToDto());
        }

        // PUT: api/Departements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartement(int id, UpdateDepartementDto updateDto)
        {
            var departement = await _context.Departements.FindAsync(id);
            if (departement == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateDto.Nom))
                departement.Nom = updateDto.Nom;

            if (updateDto.ChefDepartementId.HasValue)
            {
                var chef = await _context.ChefDepartements.FindAsync(updateDto.ChefDepartementId.Value);
                if (chef == null)
                {
                    return BadRequest("Chef de département non trouvé");
                }
                departement.ChefDepartementId = updateDto.ChefDepartementId.Value;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartementExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartement(int id)
        {
            // Utiliser un transaction pour s'assurer que toutes les opérations sont atomiques
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var departement = await _context.Departements.FindAsync(id);
                    if (departement == null)
                    {
                        return NotFound();
                    }

                    // Modification: Utiliser une requête SQL directe pour gérer le chef de département
                    if (departement.ChefDepartementId.HasValue)
                    {
                        // Désassocier le chef du département
                        await _context.Database.ExecuteSqlRawAsync(
                            "UPDATE Departements SET ChefDepartementId = NULL WHERE Id = {0}", id);

                        // Désassocier le département du chef dans la table Utilisateurs
                        await _context.Database.ExecuteSqlRawAsync(
                            "UPDATE Utilisateurs SET DepartementId = NULL WHERE Id = {0} AND TypeUtilisateur = 'ChefDepartement'",
                            departement.ChefDepartementId.Value);
                    }

                    // Mettre à null le DepartementId pour tous les encadreurs associés
                    var encadreurs = await _context.Encadreurs
                        .Where(e => e.DepartementId == id)
                        .ToListAsync();
                    foreach (var encadreur in encadreurs)
                    {
                        encadreur.DepartementId = null;
                    }
                    await _context.SaveChangesAsync();

                    // Récupérer tous les domaines associés à ce département
                    var domaines = await _context.Domaines
                        .Where(d => d.DepartementId == id)
                        .ToListAsync();

                    // Supprimer tous les domaines associés
                    _context.Domaines.RemoveRange(domaines);
                    await _context.SaveChangesAsync();

                    // Supprimer ensuite le département
                    _context.Departements.Remove(departement);
                    await _context.SaveChangesAsync();

                    // Valider la transaction
                    await transaction.CommitAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    // Annuler toutes les modifications en cas d'erreur
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Une erreur est survenue lors de la suppression: {ex.Message}");
                }
            }
        }

        private bool DepartementExists(int id)
        {
            return _context.Departements.Any(e => e.Id == id);
        }
    }
}