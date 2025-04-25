using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.MembreDirectionDTO;
using StageManager.Mapping;
using StageManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var membresDirection = await _context.MembresDirection.ToListAsync();
            return membresDirection.Select(m => m.ToDto()).ToList();
        }

        // GET: api/MembreDirection/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembreDirectionDto>> GetMembreDirection(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);

            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
            }

            return membreDirection.ToDto();
        }

        // POST: api/MembreDirection
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MembreDirectionDto>> CreateMembreDirection([FromBody] CreateMembreDirectionDto membredirectiondto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingusername = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Username == membredirectiondto.Username);
            if (existingusername != null)
            {
                return BadRequest("Le nom d'utilisateur existe déjà.");
            }

            // Vérifier si l'email existe déjà
            if (await _context.MembresDirection.AnyAsync(m => m.Email == membredirectiondto.Email))
            {
                return BadRequest($"Un membre de la direction avec l'email {membredirectiondto.Email} existe déjà.");
            }

            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<MembreDirection>();

            var membreDirection = new MembreDirection
            {
                Nom = membredirectiondto.Nom,
                Prenom = membredirectiondto.Prenom,
                Email = membredirectiondto.Email,
                Telephone = membredirectiondto.Telephone,
                Fonction = membredirectiondto.Fonction,
                Username = membredirectiondto.Username,
                MotDePasse = passwordHasher.HashPassword(null, membredirectiondto.MotDePasse),
                Role = "MembreDirection",
                DatePrisePoste = DateTime.Now,
                EstActif = true,
                DemandesDeStage = new List<DemandeDeStage>()
            };

            _context.MembresDirection.Add(membreDirection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembreDirection),
                new { id = membreDirection.Id },
                membreDirection.ToDto());
        }

        // PUT: api/MembreDirection/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembreDirection(int id, UpdateMembreDirectionDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("L'ID ne correspond pas");
            }

            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
            }

            // Vérifier si l'email est déjà utilisé par un autre membre
            if (!string.IsNullOrEmpty(updateDto.Email) &&
                await _context.MembresDirection.AnyAsync(m => m.Email == updateDto.Email && m.Id != id))
            {
                return BadRequest("Cet email est déjà utilisé par un autre membre de la direction");
            }

            // Update properties if provided
            if (!string.IsNullOrEmpty(updateDto.Nom))
                membreDirection.Nom = updateDto.Nom;

            if (!string.IsNullOrEmpty(updateDto.Prenom))
                membreDirection.Prenom = updateDto.Prenom;

            if (!string.IsNullOrEmpty(updateDto.Email))
                membreDirection.Email = updateDto.Email;

            if (!string.IsNullOrEmpty(updateDto.Telephone))
                membreDirection.Telephone = updateDto.Telephone;

            if (!string.IsNullOrEmpty(updateDto.Username))
                membreDirection.Username = updateDto.Username;

            if (!string.IsNullOrEmpty(updateDto.Fonction))
                membreDirection.Fonction = updateDto.Fonction;


            // Mise à jour du mot de passe seulement si fourni
            if (!string.IsNullOrEmpty(updateDto.MotDePasse))
            {
                var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<MembreDirection>();
                membreDirection.MotDePasse = passwordHasher.HashPassword(null, updateDto.MotDePasse);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembreDirectionExists(id))
                {
                    return NotFound("Membre de la direction non trouvé");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PATCH: api/MembreDirection/5/ToggleStatus
        [HttpPatch("{id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
            }

            membreDirection.EstActif = !membreDirection.EstActif;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/MembreDirection/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembreDirection(int id)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(id);
            if (membreDirection == null)
            {
                return NotFound("Membre de la direction non trouvé");
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
                return NotFound("Membre de la direction non trouvé");
            }

            var demandes = await _context.DemandesDeStage
                .Where(d => d.MembreDirectionId == id)
                .ToListAsync();

            return Ok(demandes);
        }

        private bool MembreDirectionExists(int id)
        {
            return _context.MembresDirection.Any(e => e.Id == id);
        }
    }
}