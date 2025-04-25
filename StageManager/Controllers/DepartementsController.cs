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
        public async Task<IActionResult> DeleteDepartement(int id)
        {
            // Find all dependent DemandesAccord records
            var demandesAccord = await _context.DemandesAccord
                .Where(d => d.ChefDepartementId == id)
                .ToListAsync();

            // Set ChefDepartementId to null
            foreach (var demande in demandesAccord)
            {
                demande.ChefDepartementId = null;
            }

            // Save changes to DemandesAccord
            await _context.SaveChangesAsync();

            // Now delete the department
            var departement = await _context.Departements.FindAsync(id);
            if (departement == null)
            {
                return NotFound();
            }

            _context.Departements.Remove(departement);
            await _context.SaveChangesAsync();

            return NoContent();
        }





        private bool DepartementExists(int id)
        {
            return _context.Departements.Any(e => e.Id == id);
        }
    }
}