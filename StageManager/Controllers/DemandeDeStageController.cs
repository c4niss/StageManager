using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DemandeDeStageDTO;
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
    public class DemandeDeStageController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DemandeDeStageController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetDemandesDeStage()
        {
            var demandes = await _db.DemandesDeStage
                .Include(d => d.Stagiaires)
                .ToListAsync();

            return Ok(demandes.Select(d => d.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDemandeDeStage(int id)
        {
            var demande = await _db.DemandesDeStage
                .Include(d => d.Stagiaires)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demande == null)
            {
                return NotFound("Demande de stage non trouvée.");
            }

            return Ok(demande.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateDemandeDeStage([FromBody] DemandeDeStageCreateDto demandeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stagiaires = await _db.Stagiaires
                .Where(s => demandeDto.StagiaireIds.Contains(s.Id))
                .ToListAsync();

            if (stagiaires.Count != demandeDto.StagiaireIds.Count)
            {
                return BadRequest("Un ou plusieurs stagiaires n'existent pas.");
            }

            var demande = new DemandeDeStage
            {
                DateDemande = DateTime.Now,
                cheminfichier = demandeDto.CheminFichier,
                Statut = DemandeDeStage.StatusDemandeDeStage.EnCour,
                Stagiaires = stagiaires
            };

            _db.DemandesDeStage.Add(demande);
            await _db.SaveChangesAsync();

            foreach (var stagiaire in stagiaires)
            {
                stagiaire.DemandeDeStageId = demande.Id;
            }
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDemandeDeStage), new { id = demande.Id }, demande.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDemandeDeStage(int id, [FromBody] DemandeDeStageUpdateDto demandeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var demande = await _db.DemandesDeStage
                .Include(d => d.Stagiaires)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demande == null)
            {
                return NotFound("Demande de stage non trouvée.");
            }

            if (!string.IsNullOrEmpty(demandeDto.CheminFichier))
            {
                demande.cheminfichier = demandeDto.CheminFichier;
            }

            demande.Statut = demandeDto.Statut;

            if (demande.Statut == DemandeDeStage.StatusDemandeDeStage.Accepte)
            {
                foreach (var stagiaire in demande.Stagiaires)
                {
                    stagiaire.Status = StagiaireStatus.Accepte;
                }
            }
            else if (demande.Statut == DemandeDeStage.StatusDemandeDeStage.Refuse)
            {
                foreach (var stagiaire in demande.Stagiaires)
                {
                    stagiaire.Status = StagiaireStatus.Refuse;
                }
            }

            if (demandeDto.StagiaireIds != null && demandeDto.StagiaireIds.Any())
            {
                var stagiaires = await _db.Stagiaires
                    .Where(s => demandeDto.StagiaireIds.Contains(s.Id))
                    .ToListAsync();

                if (stagiaires.Count != demandeDto.StagiaireIds.Count)
                {
                    return BadRequest("Un ou plusieurs stagiaires n'existent pas.");
                }

                foreach (var stagiaire in demande.Stagiaires)
                {
                    stagiaire.DemandeDeStageId = null;
                }

                demande.Stagiaires = stagiaires;
                foreach (var stagiaire in stagiaires)
                {
                    stagiaire.DemandeDeStageId = demande.Id;
                }
            }

            await _db.SaveChangesAsync();
            return Ok(demande.ToDto());
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateDemandeDeStageStatus(int id, [FromBody] DemandeDeStageUpdateStatusDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var demande = await _db.DemandesDeStage.FindAsync(id);
            if (demande == null)
            {
                return NotFound("Demande de stage non trouvée.");
            }

            demande.Statut = updateDto.Statut;

            if (demande.Statut == DemandeDeStage.StatusDemandeDeStage.Accepte)
            {
                foreach (var stagiaire in demande.Stagiaires)
                {
                    stagiaire.Status = StagiaireStatus.Accepte;
                }
            }
            else if (demande.Statut == DemandeDeStage.StatusDemandeDeStage.Refuse)
            {
                foreach (var stagiaire in demande.Stagiaires)
                {
                    stagiaire.Status = StagiaireStatus.Refuse;
                }
            }
            await _db.SaveChangesAsync();

            return Ok(demande.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDemandeDeStage(int id)
        {
            var demande = await _db.DemandesDeStage
                .Include(d => d.Stagiaires)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demande == null)
            {
                return NotFound("Demande de stage non trouvée.");
            }

            // Supprimer la référence pour chaque stagiaire
            foreach (var stagiaire in demande.Stagiaires)
            {
                stagiaire.DemandeDeStageId = null;
            }

            _db.DemandesDeStage.Remove(demande);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}