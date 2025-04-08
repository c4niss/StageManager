using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DemandeaccordDTO;
using StageManager.Mapping;
using StageManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeaccordsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DemandeaccordsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Demandeaccord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeaccordDto>>> GetDemandesAccord()
        {
            var demandes = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .ToListAsync();

            return demandes.Select(d => DemandeAccordMapping.ToDto(d)).ToList();
        }

        // GET: api/Demandeaccord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DemandeaccordDto>> GetDemandeaccord(int id)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            return DemandeAccordMapping.ToDto(demandeaccord);
        }
        // PUT: api/Demandeaccords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDemandeaccord(int id, [FromForm] UpdateDemandeaccordDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound($"La demande d'accord avec l'ID {id} n'existe pas.");
            }

            try
            {
                // Mettre à jour les dates de la période de stage
                demandeaccord.DateDebut = updateDto.DateDebut;
                demandeaccord.DateFin = updateDto.DateFin;

                // Mettre à jour les informations sur les séances
                demandeaccord.NombreSeancesParSemaine = updateDto.NombreSeancesParSemaine;
                demandeaccord.DureeSeances = updateDto.DureeSeances;

                // Mettre à jour le thème si spécifié
                if (updateDto.ThemeId.HasValue && updateDto.ThemeId != demandeaccord.ThemeId)
                {
                    var theme = await _context.Themes.FindAsync(updateDto.ThemeId.Value);
                    if (theme == null)
                    {
                        return BadRequest($"Le thème avec l'ID {updateDto.ThemeId.Value} n'existe pas.");
                    }
                    demandeaccord.ThemeId = updateDto.ThemeId.Value;
                }

                // Mettre à jour l'encadreur si spécifié
                if (updateDto.EncadreurId.HasValue && updateDto.EncadreurId != demandeaccord.EncadreurId)
                {
                    // Si l'EncadreurId est 0, on peut considérer que c'est pour retirer l'encadreur actuel
                    if (updateDto.EncadreurId == 0)
                    {
                        demandeaccord.EncadreurId = null;
                    }
                    else
                    {
                        var encadreur = await _context.Encadreurs.FindAsync(updateDto.EncadreurId.Value);
                        if (encadreur == null)
                        {
                            return BadRequest($"L'encadreur avec l'ID {updateDto.EncadreurId.Value} n'existe pas.");
                        }
                        demandeaccord.EncadreurId = updateDto.EncadreurId.Value;
                    }
                }

                _context.Entry(demandeaccord).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la mise à jour de la demande: {ex.Message}");
            }
        }
        // PUT: api/Demandeaccord/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateDemandeaccordStatus(int id, [FromBody] UpdateDemandeaccordStatusDto dto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            demandeaccord.Status = dto.Status;

            // Si un commentaire est fourni, vous pourriez l'ajouter à un champ de l'entité
            // demandeaccord.Commentaire = dto.Commentaire;

            _context.DemandesAccord.Update(demandeaccord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}