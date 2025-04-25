using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ChefDepartementDTO;
using StageManager.Mapping;
using StageManager.Models;
using System.Security.Cryptography;
using System.Text;
using TestRestApi.Data;

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
        public async Task<ActionResult<ChefDepartementDto>> CreateChefDepartement(CreateChefDepartementDto createDto)
        {
            var departement = await _context.Departements.FindAsync(createDto.DepartementId);
            if (departement == null)
            {
                return BadRequest("Département non trouvé");
            }

            var chefDepartement = new ChefDepartement
            {
                Nom = createDto.Nom,
                Prenom = createDto.Prenom,
                Username = createDto.Username,
                Email = createDto.Email,
                Telephone = createDto.Telephone,
                MotDePasse = HashPassword(createDto.MotDePasse),
                DepartementId = createDto.DepartementId,
                EstActif = createDto.EstActif,
                Role = "Chef Departement"
            };
            _context.ChefDepartements.Add(chefDepartement);
            await _context.SaveChangesAsync();

            departement.ChefDepartementId = chefDepartement.Id;
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetChefDepartement), new { id = chefDepartement.Id }, chefDepartement.ToDto());
        }

        // PUT: api/ChefDepartements/5
        [HttpPut("{id}")]
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
            chef.MotDePasse = HashPassword(updateDto.MotDePasse);

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
        public async Task<IActionResult> DeleteChefDepartement(int id)
        {
            var chef = await _context.ChefDepartements.FindAsync(id);
            if (chef == null)
            {
                return NotFound();
            }

            _context.ChefDepartements.Remove(chef);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChefDepartementExists(int id)
        {
            return _context.ChefDepartements.Any(e => e.Id == id);
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