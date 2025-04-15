using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DemandeaccordDTO;
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
    public class DemandeaccordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DemandeaccordController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Demandeaccord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeaccordDto>>> GetDemandesAccord()
        {
            var demandes = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
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
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            return DemandeAccordMapping.ToDto(demandeaccord);
        }
        // PUT: api/Demandeaccord/StagiaireUpdate/5
        [HttpPut("StagiaireUpdate/{id}")]
        public async Task<IActionResult> UpdateStagiaireDemandeaccord(int id, UpdateStagiaireDemandeaccordDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            // Vérifier si le thème existe
            var theme = await _context.Themes.FindAsync(updateDto.ThemeId);
            if (theme == null)
            {
                return BadRequest("Le thème spécifié n'existe pas.");
            }

            // Mettre à jour les propriétés du stagiaire
            demandeaccord.UniversiteInstitutEcole = updateDto.UniversiteInstitutEcole;
            demandeaccord.FiliereSpecialite = updateDto.FiliereSpecialite;
            demandeaccord.Telephone = updateDto.Telephone;
            demandeaccord.Email = updateDto.Email;
            demandeaccord.ThemeId = updateDto.ThemeId;
            demandeaccord.DiplomeObtention = updateDto.DiplomeObtention;
            demandeaccord.NatureStage = updateDto.NatureStage;
            demandeaccord.Nom = updateDto.Nom;
            demandeaccord.Prenom = updateDto.Prenom;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id))
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

        // PUT: api/Demandeaccord/ChefDepartementUpdate/5
        [HttpPut("ChefDepartementUpdate/{id}")]
        public async Task<IActionResult> UpdateChefDepartementDemandeaccord(int id, UpdateChefDepartementDemandeaccordDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            // Vérifier que la date de fin est après la date de début
            if (updateDto.DateFin <= updateDto.DateDebut)
            {
                return BadRequest("La date de fin doit être postérieure à la date de début.");
            }

            // Mettre à jour les propriétés par le chef de département
            demandeaccord.ServiceAccueil = updateDto.ServiceAccueil;
            demandeaccord.DateDebut = updateDto.DateDebut;
            demandeaccord.DateFin = updateDto.DateFin;
            demandeaccord.NombreSeancesParSemaine = updateDto.NombreSeancesParSemaine;
            demandeaccord.DureeSeances = updateDto.DureeSeances;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id))
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

        // PUT: api/Demandeaccord/UpdateStatus/5
        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateDemandeaccordStatusDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            demandeaccord.Status = updateDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id))
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

        // PUT: api/Demandeaccord/AssignEncadreur/5?encadreurId=10
        [HttpPut("AssignEncadreur/{id}")]
        public async Task<IActionResult> AssignEncadreur(int id, [FromQuery] int encadreurId)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            var encadreur = await _context.Encadreurs.FindAsync(encadreurId);
            if (encadreur == null)
            {
                return BadRequest("L'encadreur spécifié n'existe pas.");
            }

            demandeaccord.EncadreurId = encadreurId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id))
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
        [HttpDelete("{id}")]
        // DELETE: api/Demandeaccord/5
        public async Task<IActionResult> DeleteDemandeacoord(int id)
        {
            var demandeaccord = await _context.DemandesAccord.FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null)
            {
                return NotFound();
            }
            _context.DemandesAccord.Remove(demandeaccord);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool DemandeaccordExists(int id)
        {
            return _context.DemandesAccord.Any(e => e.Id == id);
        }

    }
}