using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ConventionDTO;
using StageManager.Mapping;
using StageManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;
using static StageManager.Models.Convention;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConventionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConventionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Convention
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConventionDto>>> GetConventions()
        {
            var conventions = await _context.Conventions
                .Include(c => c.Stage)
                .Include(c => c.MembreDirection)
                .ToListAsync();

            return conventions.Select(c => c.ToDto()).ToList();
        }

        // GET: api/Convention/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConventionDto>> GetConvention(int id)
        {
            var convention = await _context.Conventions
                .Include(c => c.Stage)
                .Include(c => c.MembreDirection)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (convention == null)
            {
                return NotFound();
            }

            return convention.ToDto();
        }

        // GET: api/Convention/ByStage/5
        [HttpGet("ByStage/{stageId}")]
        public async Task<ActionResult<IEnumerable<ConventionDto>>> GetConventionsByStage(int stageId)
        {
            var conventions = await _context.Conventions
                .Include(c => c.Stage)
                .Include(c => c.MembreDirection)
                .Where(c => c.StageId == stageId)
                .ToListAsync();

            return conventions.Select(c => c.ToDto()).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<ConventionDto>> CreateConvention(CreateConventionDto createConventionDto)
        {
            var membreDirection = await _context.MembresDirection.FindAsync(createConventionDto.MembreDirectionId);
            if (membreDirection == null)
            {
                return BadRequest("Le membre de direction spécifié n'existe pas.");
            }

            var convention = new Convention
            {
                DateDepot = createConventionDto.DateCreation,
                CheminFichier = createConventionDto.CheminFichier,
                status = Statusconvention.EnCours,
                MembreDirectionId = createConventionDto.MembreDirectionId,
                StageId = createConventionDto.StageId
            };

            _context.Conventions.Add(convention);
            await _context.SaveChangesAsync();

            // Recharger la convention avec les relations
            convention = await _context.Conventions
                .Include(c => c.Stage)
                .Include(c => c.MembreDirection)
                .FirstOrDefaultAsync(c => c.Id == convention.Id);

            return CreatedAtAction(nameof(GetConvention), new { id = convention.Id }, convention.ToDto());
        }

        // PUT: api/Convention/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConvention(int id, UpdateConventionDto updateConventionDto)
        {
            var convention = await _context.Conventions.FindAsync(id);
            if (convention == null)
            {
                return NotFound();
            }

            var membreDirection = await _context.MembresDirection.FindAsync(updateConventionDto.MembreDirectionId);
            if (membreDirection == null)
            {
                return BadRequest("Le membre de direction spécifié n'existe pas.");
            }

            // Vérifier si StageId est fourni et existe
            if (updateConventionDto.StageId.HasValue)
            {
                var stage = await _context.Stages.FindAsync(updateConventionDto.StageId.Value);
                if (stage == null)
                {
                    return BadRequest("Le stage spécifié n'existe pas.");
                }
                convention.StageId = updateConventionDto.StageId;
            }

            convention.CheminFichier = updateConventionDto.CheminFichier;
            convention.MembreDirectionId = updateConventionDto.MembreDirectionId;
            convention.status = updateConventionDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConventionExists(id))
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

        // DELETE: api/Convention/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConvention(int id)
        {
            var convention = await _context.Conventions.FindAsync(id);
            if (convention == null)
            {
                return NotFound();
            }

            _context.Conventions.Remove(convention);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ConventionExists(int id)
        {
            return _context.Conventions.Any(e => e.Id == id);
        }
    }
}