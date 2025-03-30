
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.MembreDirectionDTO;
using StageManager.DTO.StagiaireDTO;
using StageManager.Models;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembreDirectionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MembreDirectionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MembreDirection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembreDirectionDto>>> GetMembresDirection()
        {
            // Remarque: votre DTO MembreDirectionDto est vide, vous devrez l'enrichir
            // Je vais créer une version temporaire ici
            var membresDirection = await _context.MembresDirection.ToListAsync();

            return membresDirection.Select(m => new
            {
                Id = m.Id,
                Nom = m.Nom,
                Prenom = m.Prenom,
                Email = m.Email,
                Telephone = m.Telephone,
                Fonction = m.Fonction,
                DatePrisePoste = m.DatePrisePoste
            }).Cast<MembreDirectionDto>().ToList();
        }

        // GET: api/MembreDirection/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembreDirectionDto>> GetMembreDirection(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);

            if (membreDirection == null)
            {
                return NotFound();
            }

            // Comme le DTO est vide, créer un objet anonyme (à remplacer par un vrai DTO)
            return Ok(new
            {
                Id = membreDirection.Id,
                Nom = membreDirection.Nom,
                Prenom = membreDirection.Prenom,
                Email = membreDirection.Email,
                Telephone = membreDirection.Telephone,
                Fonction = membreDirection.Fonction,
                DatePrisePoste = membreDirection.DatePrisePoste
            });
        }

        // POST: api/MembreDirection
        [HttpPost]
        public async Task<ActionResult<MembreDirectionDto>> CreateMembreDirection(CreateMembreDirectionDto dto)
        {
            // Vérifier si l'email existe déjà
            var emailExists = await _context.MembresDirection.AnyAsync(m => m.Email == dto.Email);
            if (emailExists)
            {
                return BadRequest("Un membre de la direction avec cet email existe déjà");
            }

            var membreDirection = new MembreDirection
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Fonction = dto.Fonction,
                MotDePasse = dto.MotDePasse, // Note: In a real app, hash this password
                Role = "MembreDirection",
                EstActif = true,
                DatePrisePoste = DateTime.UtcNow
            };

            _context.MembresDirection.Add(membreDirection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembreDirection), new { id = membreDirection.Id }, membreDirection);
        }

        // PUT: api/MembreDirection/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembreDirection(int id, CreateMembreDirectionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound();
            }

            // Update properties
            membreDirection.Nom = dto.Nom;
            membreDirection.Prenom = dto.Prenom;
            membreDirection.Email = dto.Email;
            membreDirection.Fonction = dto.Fonction;
            // Only update password if provided
            if (!string.IsNullOrEmpty(dto.MotDePasse))
            {
                membreDirection.MotDePasse = dto.MotDePasse; // Should be hashed
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembreDirectionExists(id))
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

        // DELETE: api/MembreDirection/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembreDirection(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound();
            }

            _context.MembresDirection.Remove(membreDirection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/MembreDirection/5/DemandesDeStage
        [HttpGet("{id}/DemandesDeStage")]
        public async Task<ActionResult<IEnumerable<DemandeDeStage>>> GetDemandesDeStage(int id)
        {
            if (!MembreDirectionExists(id))
            {
                return NotFound();
            }

            var demandes = await _context.DemandesDeStage
                .Where(d => d.MembreDirectionId == id)
                .ToListAsync();

            return demandes;
        }

        // GET: api/MembreDirection/5/Conventions
        [HttpGet("{id}/Conventions")]
        public async Task<ActionResult<IEnumerable<Convention>>> GetConventions(int id)
        {
            if (!MembreDirectionExists(id))
            {
                return NotFound();
            }

            var conventions = await _context.Conventions
                .Where(c => c.MembreDirectionId == id)
                .ToListAsync();

            return conventions;
        }

        // POST: api/MembreDirection/Login
        [HttpPost("Login")]
        public async Task<ActionResult<MembreDirection>> Login(StagiaireloginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membreDirection = await _context.MembresDirection
                .FirstOrDefaultAsync(m => m.Email == dto.Email && m.MotDePasse == dto.MotDePasse);

            if (membreDirection == null)
            {
                return Unauthorized();
            }

            // In a real app, generate and return a JWT token
            return Ok(membreDirection);
        }

        private bool MembreDirectionExists(int id)
        {
            return _context.MembresDirection.Any(e => e.Id == id);
        }
    }
}