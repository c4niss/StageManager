using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
    public class MemoireController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MemoireController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Memoire
        [HttpGet]
        [Authorize(Roles = "Direction,Encadreur")]
        public async Task<ActionResult<IEnumerable<MemoireListDto>>> GetMemoires()
        {
            var memoires = await _context.Memoires
                .Include(m => m.Stage)
                    .ThenInclude(s => s.Stagiaires)
                .Include(m => m.DemandeDepotMemoire)
                    .ThenInclude(d => d.Theme)
                .Select(m => new MemoireListDto
                {
                    Id = m.Id,
                    Titre = m.Titre,
                    DateDepot = m.DateDepot,
                    NomPrenomStagiaire = string.Join(", ", m.Stage.Stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
                    ThemeStage = m.DemandeDepotMemoire.Theme.Nom
                })
                .ToListAsync();

            return memoires;
        }

        // GET: api/Memoire/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Direction,Encadreur,Stagiaire")]
        public async Task<ActionResult<MemoireReadDto>> GetMemoire(int id)
        {
            var memoire = await _context.Memoires
                .Include(m => m.Stage)
                    .ThenInclude(s => s.Stagiaires)
                .Include(m => m.DemandeDepotMemoire)
                    .ThenInclude(d => d.Theme)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (memoire == null)
            {
                return NotFound();
            }

            return new MemoireReadDto
            {
                Id = memoire.Id,
                Titre = memoire.Titre,
                CheminFichier = memoire.CheminFichier,
                DateDepot = memoire.DateDepot,
                DemandeDepotMemoireId = memoire.DemandeDepotMemoireId,
                StageId = memoire.StageId,
                NomPrenomStagiaire = string.Join(", ", memoire.Stage.Stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
                ThemeStage = memoire.DemandeDepotMemoire.Theme.Nom
            };
        }

        // POST: api/Memoire
        [HttpPost]
        [Authorize(Roles = "Stagiaire")]
        public async Task<ActionResult<MemoireReadDto>> CreateMemoire(MemoireCreateDto memoireDto)
        {
            // Vérifier si la demande de dépôt existe
            var demandeDepot = await _context.DemandesDepotMemoire
                .Include(d => d.Theme)
                .FirstOrDefaultAsync(d => d.Id == memoireDto.DemandeDepotMemoireId);

            if (demandeDepot == null)
            {
                return BadRequest("Demande de dépôt de mémoire introuvable");
            }

            // Vérifier si le stage existe
            var stage = await _context.Stages
                .Include(s => s.Stagiaires)
                .FirstOrDefaultAsync(s => s.Id == memoireDto.StageId);

            if (stage == null)
            {
                return BadRequest("Stage introuvable");
            }

            // Vérifier que la demande est validée
            if (demandeDepot.Statut != StatutDepotMemoire.Valide)
            {
                return BadRequest("La demande de dépôt n'a pas encore été validée");
            }

            // Vérifier qu'un mémoire n'existe pas déjà pour ce stage
            var existingMemoire = await _context.Memoires
                .FirstOrDefaultAsync(m => m.StageId == memoireDto.StageId);

            if (existingMemoire != null)
            {
                return Conflict("Un mémoire existe déjà pour ce stage");
            }

            var memoire = new Memoire
            {
                Titre = memoireDto.Titre,
                CheminFichier = memoireDto.CheminFichier,
                DateDepot = DateTime.Now,
                DemandeDepotMemoireId = memoireDto.DemandeDepotMemoireId,
                StageId = memoireDto.StageId
            };

            _context.Memoires.Add(memoire);
            await _context.SaveChangesAsync();

            // Recharger l'entité avec ses relations pour le DTO de retour
            memoire = await _context.Memoires
                .Include(m => m.Stage)
                    .ThenInclude(s => s.Stagiaires)
                .Include(m => m.DemandeDepotMemoire)
                    .ThenInclude(d => d.Theme)
                .FirstOrDefaultAsync(m => m.Id == memoire.Id);

            return CreatedAtAction(nameof(GetMemoire), new { id = memoire.Id },
                new MemoireReadDto
                {
                    Id = memoire.Id,
                    Titre = memoire.Titre,
                    CheminFichier = memoire.CheminFichier,
                    DateDepot = memoire.DateDepot,
                    DemandeDepotMemoireId = memoire.DemandeDepotMemoireId,
                    StageId = memoire.StageId,
                    NomPrenomStagiaire = string.Join(", ", memoire.Stage.Stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
                    ThemeStage = memoire.DemandeDepotMemoire.Theme.Nom
                });
        }

        // PUT: api/Memoire/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Stagiaire")]
        public async Task<IActionResult> UpdateMemoire(int id, MemoireUpdateDto memoireDto)
        {
            if (id != memoireDto.Id)
            {
                return BadRequest();
            }

            var memoire = await _context.Memoires.FindAsync(id);

            if (memoire == null)
            {
                return NotFound();
            }

            // Vérifier que le mémoire n'a pas été déposé depuis plus de 48 heures
            if ((DateTime.Now - memoire.DateDepot).TotalHours > 48)
            {
                return BadRequest("Le mémoire ne peut plus être modifié après 48 heures");
            }

            memoire.Titre = memoireDto.Titre;

            if (!string.IsNullOrEmpty(memoireDto.CheminFichier))
            {
                memoire.CheminFichier = memoireDto.CheminFichier;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemoireExists(id))
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

        // DELETE: api/Memoire/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Direction,Stagiaire")]
        public async Task<IActionResult> DeleteMemoire(int id)
        {
            var memoire = await _context.Memoires.FindAsync(id);

            if (memoire == null)
            {
                return NotFound();
            }

            // Vérifier que le mémoire n'a pas été déposé depuis plus de 48 heures (pour les stagiaires)
            if (User.IsInRole("Stagiaire") && (DateTime.Now - memoire.DateDepot).TotalHours > 48)
            {
                return BadRequest("Le mémoire ne peut plus être supprimé après 48 heures");
            }

            _context.Memoires.Remove(memoire);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Memoire/Stage/5
        [HttpGet("Stage/{stageId}")]
        [Authorize(Roles = "Direction,Encadreur,Stagiaire")]
        public async Task<ActionResult<MemoireReadDto>> GetMemoireByStage(int stageId)
        {
            var memoire = await _context.Memoires
                .Include(m => m.Stage)
                    .ThenInclude(s => s.Stagiaires)
                .Include(m => m.DemandeDepotMemoire)
                    .ThenInclude(d => d.Theme)
                .FirstOrDefaultAsync(m => m.StageId == stageId);

            if (memoire == null)
            {
                return NotFound();
            }

            return new MemoireReadDto
            {
                Id = memoire.Id,
                Titre = memoire.Titre,
                CheminFichier = memoire.CheminFichier,
                DateDepot = memoire.DateDepot,
                DemandeDepotMemoireId = memoire.DemandeDepotMemoireId,
                StageId = memoire.StageId,
                NomPrenomStagiaire = string.Join(", ", memoire.Stage.Stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
                ThemeStage = memoire.DemandeDepotMemoire.Theme.Nom
            };
        }

        // GET: api/Memoire/DemandeDepot/5
        [HttpGet("DemandeDepot/{demandeDepotId}")]
        [Authorize(Roles = "Direction,Encadreur,Stagiaire")]
        public async Task<ActionResult<MemoireReadDto>> GetMemoireByDemandeDepot(int demandeDepotId)
        {
            var memoire = await _context.Memoires
                .Include(m => m.Stage)
                    .ThenInclude(s => s.Stagiaires)
                .Include(m => m.DemandeDepotMemoire)
                    .ThenInclude(d => d.Theme)
                .FirstOrDefaultAsync(m => m.DemandeDepotMemoireId == demandeDepotId);

            if (memoire == null)
            {
                return NotFound();
            }

            return new MemoireReadDto
            {
                Id = memoire.Id,
                Titre = memoire.Titre,
                CheminFichier = memoire.CheminFichier,
                DateDepot = memoire.DateDepot,
                DemandeDepotMemoireId = memoire.DemandeDepotMemoireId,
                StageId = memoire.StageId,
                NomPrenomStagiaire = string.Join(", ", memoire.Stage.Stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
                ThemeStage = memoire.DemandeDepotMemoire.Theme.Nom
            };
        }

        private bool MemoireExists(int id)
        {
            return _context.Memoires.Any(e => e.Id == id);
        }
    }
}
