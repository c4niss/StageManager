using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ConventionDTO;
using StageManager.DTO.DemandeDeStageDTO;
using StageManager.DTO.LoginDTO;
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
            return membresDirection.Select(m => MembreDirectionMapping.ToDto(m)).ToList();
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

            return MembreDirectionMapping.ToDto(membreDirection);
        }

        // POST: api/MembreDirection
        [HttpPost]
        public async Task<ActionResult<MembreDirectionDto>> CreateMembreDirection(CreateMembreDirectionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Vérifier si l'email existe déjà
            var emailExists = await _context.MembresDirection.AnyAsync(m => m.Email == dto.Email);
            if (emailExists)
            {
                return BadRequest("Un membre de la direction avec cet email existe déjà");
            }

            var membreDirection = MembreDirectionMapping.ToEntity(dto);
            membreDirection.Role = "MembreDirection";
            membreDirection.EstActif = true;
            membreDirection.DatePrisePoste = DateTime.UtcNow;
            membreDirection.DateCreation = DateTime.UtcNow;

            // Hachage du mot de passe avec PasswordHasher
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<MembreDirection>();
            membreDirection.MotDePasse = passwordHasher.HashPassword(null, dto.MotDePasse);

            _context.MembresDirection.Add(membreDirection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembreDirection),
                new { id = membreDirection.Id },
                MembreDirectionMapping.ToDto(membreDirection));
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
                return NotFound("Membre de la direction non trouvé");
            }

            // Vérifier si l'email est déjà utilisé par un autre membre
            var emailExists = await _context.MembresDirection
                .AnyAsync(m => m.Email == dto.Email && m.Id != id);
            if (emailExists)
            {
                return BadRequest("Cet email est déjà utilisé par un autre membre de la direction");
            }

            // Mise à jour des propriétés
            membreDirection.Nom = dto.Nom;
            membreDirection.Prenom = dto.Prenom;
            membreDirection.Email = dto.Email;
            membreDirection.Fonction = dto.Fonction;

            // Mise à jour du mot de passe seulement si fourni
            if (!string.IsNullOrEmpty(dto.MotDePasse))
            {
                var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<MembreDirection>();
                membreDirection.MotDePasse = passwordHasher.HashPassword(null, dto.MotDePasse);
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

            // Vérifier si le membre a des références
            var hasConventions = await _context.Conventions.AnyAsync(c => c.MembreDirectionId == id);
            var hasDemandesStage = await _context.DemandesDeStage.AnyAsync(d => d.MembreDirectionId == id);
            var hasAttestations = await _context.Attestations.AnyAsync(a => a.MembreDirectionId == id);
            var hasDemandesMemoire = await _context.DemandesDepotMemoire.AnyAsync(d => d.MembreDirectionId == id);

            if (hasConventions || hasDemandesStage || hasAttestations || hasDemandesMemoire)
            {
                return BadRequest("Ce membre de la direction ne peut pas être supprimé car il est référencé par d'autres entités");
            }

            _context.MembresDirection.Remove(membreDirection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/MembreDirection/5/DemandesDeStage
        [HttpGet("{id}/DemandesDeStage")]
        public async Task<ActionResult<IEnumerable<DemandeDeStageDto>>> GetDemandesDeStage(int id)
        {
            if (!MembreDirectionExists(id))
            {
                return NotFound("Membre de la direction non trouvé");
            }

            var demandes = await _context.DemandesDeStage
                .Where(d => d.MembreDirectionId == id)
                .Include(d => d.Stagiaires)
                .ToListAsync();

            return demandes.Select(d => DemandeDeStageMapping.ToDto(d)).ToList();
        }

        // GET: api/MembreDirection/5/Conventions
        [HttpGet("{id}/Conventions")]
        public async Task<ActionResult<IEnumerable<ConventionDto>>> GetConventions(int id)
        {
            if (!MembreDirectionExists(id))
            {
                return NotFound("Membre de la direction non trouvé");
            }

            var conventions = await _context.Conventions
                .Where(c => c.MembreDirectionId == id)
                .Include(c => c.MembreDirection)
                .Include(c => c.Stage)
                .ToListAsync();

            return conventions.Select(c => ConventionMapping.ToDto(c)).ToList();
        }

        // GET: api/MembreDirection/DashboardStats
        [HttpGet("DashboardStats")]
        public async Task<ActionResult<object>> GetDashboardStats()
        {
            var totalMembres = await _context.MembresDirection.CountAsync();
            var totalConventions = await _context.Conventions.CountAsync();
            var conventionsValidees = await _context.Conventions.CountAsync(c => c.EstValidee);
            var totalAttestations = await _context.Attestations.CountAsync();
            var attestationsDelivrees = await _context.Attestations.CountAsync(a => a.EstDelivree);

            var stats = new
            {
                TotalMembres = totalMembres,
                TotalConventions = totalConventions,
                ConventionsValidees = conventionsValidees,
                TauxValidationConventions = totalConventions > 0 ? (double)conventionsValidees / totalConventions * 100 : 0,
                TotalAttestations = totalAttestations,
                AttestationsDelivrees = attestationsDelivrees,
                TauxDelivranceAttestations = totalAttestations > 0 ? (double)attestationsDelivrees / totalAttestations * 100 : 0
            };

            return Ok(stats);
        }

        // POST: api/MembreDirection/Login
        [HttpPost("Login")]
        public async Task<ActionResult<MembreDirectionDto>> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membreDirection = await _context.MembresDirection
                .FirstOrDefaultAsync(m => m.Email == dto.Email);

            if (membreDirection == null)
            {
                return Unauthorized("Email ou mot de passe incorrect");
            }

            // Vérifier le mot de passe avec PasswordHasher
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<MembreDirection>();
            var result = passwordHasher.VerifyHashedPassword(null, membreDirection.MotDePasse, dto.MotDePasse);

            if (result != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                return Unauthorized("Email ou mot de passe incorrect");
            }

            // Vérifier si le compte est actif
            if (!membreDirection.EstActif)
            {
                return Unauthorized("Ce compte est désactivé");
            }

            return Ok(MembreDirectionMapping.ToDto(membreDirection));
        }

        private bool MembreDirectionExists(int id)
        {
            return _context.MembresDirection.Any(e => e.Id == id);
        }
    }
}