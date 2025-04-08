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

        // POST: api/Demandeaccord
        [HttpPost]
        public async Task<ActionResult<DemandeaccordDto>> CreateDemandeaccord([FromForm] CreateDemandeaccordDto dto)
        {
            // Vérification des stagiaires
            var stagiaires = new List<Stagiaire>();
            foreach (var stagiaireId in dto.StagiaireId)
            {
                var stagiaire = await _context.Stagiaires.FindAsync(stagiaireId);
                if (stagiaire == null)
                {
                    return BadRequest($"Stagiaire avec l'ID {stagiaireId} non trouvé");
                }
                stagiaires.Add(stagiaire);
            }

            // Vérification du thème
            var theme = await _context.Themes.FindAsync(dto.ThemeId);
            if (theme == null)
            {
                return BadRequest("Thème non trouvé");
            }

            // Vérification de l'encadreur si fourni
            Encadreur encadreur = null;
            if (dto.EncadreurId.HasValue)
            {
                encadreur = await _context.Encadreurs.FindAsync(dto.EncadreurId);
                if (encadreur == null)
                {
                    return BadRequest("Encadreur non trouvé");
                }
            }

            // Gérer le fichier joint
            string uniqueFileName = null;
            if (dto.FichePieceJointe != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "demandes");
                Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.FichePieceJointe.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.FichePieceJointe.CopyToAsync(fileStream);
                }
            }

            // Créer la demande d'accord en utilisant le mapping
            var demandeaccord = DemandeAccordMapping.ToEntity(dto);
            demandeaccord.FichePieceJointe = uniqueFileName;
            demandeaccord.stagiaires = stagiaires;
            demandeaccord.DemandeStageId = 0; // À définir selon votre logique

            _context.DemandesAccord.Add(demandeaccord);
            await _context.SaveChangesAsync();

            // Récupérer la demande complète avec ses relations
            var createdDemandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == demandeaccord.Id);

            return CreatedAtAction(
                nameof(GetDemandeaccord),
                new { id = createdDemandeaccord.Id },
                DemandeAccordMapping.ToDto(createdDemandeaccord));
        }

        // PUT: api/Demandeaccord/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateDemandeaccordStatus(int id, UpdateDemandeaccordStatusDto dto)
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