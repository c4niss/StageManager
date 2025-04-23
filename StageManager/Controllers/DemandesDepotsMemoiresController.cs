using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StageManager.Models;
using StageManager.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;
using StageManager.DTO.MembreDirectionDTO;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeDepotMemoireController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DemandeDepotMemoireController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DemandeDepotMemoire
        [HttpGet]
        [Authorize(Roles = "Direction,Encadreur")]
        public async Task<ActionResult<IEnumerable<DemandeDepotMemoireListDto>>> GetDemandesDepotMemoire()
        {
            var demandes = await _context.DemandesDepotMemoire
                .Include(d => d.Stage)
                    .ThenInclude(s => s.Convention)
                        .ThenInclude(c => c.DemandeAccord)
                            .ThenInclude(da => da.Theme)
                .Include(d => d.Encadreur)
                .Select(d => new DemandeDepotMemoireListDto
                {
                    Id = d.Id,
                    DateDemande = d.DateDemande,
                    Statut = d.Statut,
                    NomTheme = d.Theme.Nom,
                    NomPrenomEtudiants = d.NomPrenomEtudiants,
                    NomPrenomEncadreur = d.NomPrenomEncadreur,
                    DateDebutStage = d.Stage.DateDebut,
                    DateFinStage = d.Stage.DateFin,
                    EstValideParDirection = d.DateValidation.HasValue
                })
                .ToListAsync();

            return demandes;
        }

        // GET: api/DemandeDepotMemoire/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Direction,Encadreur,Stagiaire")]
        public async Task<ActionResult<DemandeDepotMemoireReadDto>> GetDemandeDepotMemoire(int id)
        {
            var demande = await _context.DemandesDepotMemoire
                .Include(d => d.Theme)
                .Include(d => d.Stage)
                    .ThenInclude(s => s.Stagiaires)
                .Include(d => d.Encadreur)
                .Include(d => d.MembreDirection)
                .Include(d => d.Memoire)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demande == null)
            {
                return NotFound();
            }

            return new DemandeDepotMemoireReadDto
            {
                Id = demande.Id,
                DateDemande = demande.DateDemande,
                Statut = demande.Statut,

                // Informations sur le thème
                ThemeId = demande.themeId,
                NomTheme = demande.Theme.Nom,

                // Informations sur les étudiants
                NomPrenomEtudiants = demande.NomPrenomEtudiants,

                // Informations sur l'encadreur
                EncadreurId = demande.EncadreurId,
                NomPrenomEncadreur = demande.NomPrenomEncadreur,
                FonctionEncadreur = demande.Encadreur.Fonction,

                // Informations sur le stage
                StageId = demande.StageId,
                DateDebutStage = demande.Stage.DateDebut,
                DateFinStage = demande.Stage.DateFin,
                StagiaireGroup = demande.Stage.StagiaireGroup,

                // Informations sur la validation
                EstValideParDirection = demande.DateValidation.HasValue,
                DateValidation = demande.DateValidation,

                MembreDirection = demande.MembreDirection != null ? new MinimalMembreDirectionDto
                {
                    Id = demande.MembreDirection.Id,
                    Nom = demande.MembreDirection.Nom ,
                    Prenom = demande.MembreDirection.Prenom,
                    Fonction = demande.MembreDirection.Fonction
                } : null,
                Memoire = demande.Memoire != null ? new MemoireMinimalDto
                {
                    Id = demande.Memoire.Id,
                    Titre = demande.Memoire.Titre,
                    DateDepot = demande.Memoire.DateDepot
                } : null
            };
        }

        // POST: api/DemandeDepotMemoire
        [HttpPost]
        [Authorize(Roles = "Encadreur")]
        public async Task<ActionResult<DemandeDepotMemoireReadDto>> CreateDemandeDepotMemoire(DemandeDepotMemoireCreateDto demandeDto)
        {
            // Vérifier si le stage existe
            var stage = await _context.Stages
                .Include(s => s.Convention)
                    .ThenInclude(c => c.DemandeAccord)
                        .ThenInclude(da => da.Theme)
                .FirstOrDefaultAsync(s => s.Id == demandeDto.StageId);

            if (stage == null)
            {
                return BadRequest("Stage introuvable");
            }

            // Vérifier si l'encadreur existe
            var encadreur = await _context.Encadreurs.FindAsync(demandeDto.EncadreurId);
            if (encadreur == null)
            {
                return BadRequest("Encadreur introuvable");
            }

            // Vérifier si l'encadreur est bien l'encadreur du stage
            if (stage.EncadreurId != demandeDto.EncadreurId)
            {
                return BadRequest("L'encadreur spécifié n'est pas l'encadreur du stage");
            }

            // Vérifier si une demande existe déjà pour ce stage
            var existingDemande = await _context.DemandesDepotMemoire
                .FirstOrDefaultAsync(d => d.StageId == demandeDto.StageId);

            if (existingDemande != null)
            {
                return Conflict("Une demande de dépôt de mémoire existe déjà pour ce stage");
            }

            // Récupérer le thème associé à la demande d'accord du stage
            var theme = stage.Convention.DemandeAccord.Theme;
            if (theme == null)
            {
                return BadRequest("Aucun thème n'est associé à ce stage");
            }

            // Récupérer les noms des stagiaires associés au stage
            string nomPrenomEtudiants = string.Join(", ", stage.Stagiaires.Select(s => $"{s.Nom} {s.Prenom}"));

            var demande = new DemandeDepotMemoire
            {
                DateDemande = DateTime.Now,
                Statut = StatutDepotMemoire.EnAttente,
                themeId = theme.Id,
                Theme = theme,
                NomPrenomEtudiants = nomPrenomEtudiants,
                NomPrenomEncadreur = $"{encadreur.Nom} {encadreur.Prenom}",
                StageId = demandeDto.StageId,
                EncadreurId = demandeDto.EncadreurId
            };

            _context.DemandesDepotMemoire.Add(demande);
            await _context.SaveChangesAsync();

            // Recharger l'entité avec ses relations pour le DTO de retour
            demande = await _context.DemandesDepotMemoire
                .Include(d => d.Theme)
                .Include(d => d.Stage)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == demande.Id);

            return CreatedAtAction(nameof(GetDemandeDepotMemoire), new { id = demande.Id },
                new DemandeDepotMemoireReadDto
                {
                    Id = demande.Id,
                    DateDemande = demande.DateDemande,
                    Statut = demande.Statut,
                    ThemeId = demande.themeId,
                    NomTheme = demande.Theme.Nom,
                    NomPrenomEtudiants = demande.NomPrenomEtudiants,
                    EncadreurId = demande.EncadreurId,
                    NomPrenomEncadreur = demande.NomPrenomEncadreur,
                    FonctionEncadreur = demande.Encadreur.Fonction,
                    StageId = demande.StageId,
                    DateDebutStage = demande.Stage.DateDebut,
                    DateFinStage = demande.Stage.DateFin,
                    StagiaireGroup = demande.Stage.StagiaireGroup,
                    EstValideParDirection = false
                });
        }

        // PUT: api/DemandeDepotMemoire/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Encadreur")]
        public async Task<IActionResult> UpdateDemandeDepotMemoire(int id, DemandeDepotMemoireUpdateDto demandeDto)
        {
            if (id != demandeDto.Id)
            {
                return BadRequest();
            }

            var demande = await _context.DemandesDepotMemoire.FindAsync(id);

            if (demande == null)
            {
                return NotFound();
            }

            // Vérifier que la demande est toujours en attente
            if (demande.Statut != StatutDepotMemoire.EnAttente)
            {
                return BadRequest("Impossible de modifier une demande qui n'est plus en attente");
            }


            // Si le stage a changé, mettre à jour les relations
            if (demandeDto.stageId != 0 && demandeDto.stageId != demande.StageId)
            {
                var stage = await _context.Stages
                    .Include(s => s.Convention)
                        .ThenInclude(c => c.DemandeAccord)
                            .ThenInclude(da => da.Theme)
                    .FirstOrDefaultAsync(s => s.Id == demandeDto.stageId);

                if (stage == null)
                {
                    return BadRequest("Stage introuvable");
                }

                // Vérifier que l'encadreur est bien l'encadreur du stage
                if (stage.EncadreurId != demande.EncadreurId)
                {
                    return BadRequest("L'encadreur de la demande n'est pas l'encadreur du nouveau stage");
                }

                // Mettre à jour le stage et le thème associé
                demande.StageId = demandeDto.stageId;
                demande.themeId = stage.Convention.DemandeAccord.Theme.Id;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeDepotMemoireExists(id))
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

        // POST: api/DemandeDepotMemoire/Valider/5
        [HttpPost("Valider/{id}")]
        [Authorize(Roles = "Direction")]
        public async Task<IActionResult> ValiderDemandeDepotMemoire(int id, [FromBody] ValidationDemandeDto validationDto)
        {
            var demande = await _context.DemandesDepotMemoire.FindAsync(id);

            if (demande == null)
            {
                return NotFound();
            }

            // Vérifier que la demande est toujours en attente
            if (demande.Statut != StatutDepotMemoire.EnAttente)
            {
                return BadRequest("Cette demande a déjà été traitée");
            }

            demande.Statut = validationDto.EstApprouve ? StatutDepotMemoire.Valide : StatutDepotMemoire.Rejete;
            demande.DateValidation = DateTime.Now;
            demande.MembreDirectionId = validationDto.MembreDirectionId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeDepotMemoireExists(id))
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

        // DELETE: api/DemandeDepotMemoire/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Direction,Encadreur")]
        public async Task<IActionResult> DeleteDemandeDepotMemoire(int id)
        {
            var demande = await _context.DemandesDepotMemoire.FindAsync(id);

            if (demande == null)
            {
                return NotFound();
            }

            // Vérifier que la demande est toujours en attente (pour les encadreurs)
            if (User.IsInRole("Encadreur") && demande.Statut != StatutDepotMemoire.EnAttente)
            {
                return BadRequest("Impossible de supprimer une demande qui n'est plus en attente");
            }

            _context.DemandesDepotMemoire.Remove(demande);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/DemandeDepotMemoire/Stage/5
        [HttpGet("Stage/{stageId}")]
        [Authorize(Roles = "Direction,Encadreur,Stagiaire")]
        public async Task<ActionResult<DemandeDepotMemoireReadDto>> GetDemandeDepotMemoireByStage(int stageId)
        {
            var demande = await _context.DemandesDepotMemoire
                .Include(d => d.Theme)
                .Include(d => d.Stage)
                .Include(d => d.Encadreur)
                .Include(d => d.MembreDirection)
                .Include(d => d.Memoire)
                .FirstOrDefaultAsync(d => d.StageId == stageId);

            if (demande == null)
            {
                return NotFound();
            }

            return new DemandeDepotMemoireReadDto
            {
                Id = demande.Id,
                DateDemande = demande.DateDemande,
                Statut = demande.Statut,
                ThemeId = demande.themeId,
                NomTheme = demande.Theme.Nom,
                NomPrenomEtudiants = demande.NomPrenomEtudiants,
                EncadreurId = demande.EncadreurId,
                NomPrenomEncadreur = demande.NomPrenomEncadreur,
                FonctionEncadreur = demande.Encadreur.Fonction,
                StageId = demande.StageId,
                DateDebutStage = demande.Stage.DateDebut,
                DateFinStage = demande.Stage.DateFin,
                StagiaireGroup = demande.Stage.StagiaireGroup,
                EstValideParDirection = demande.DateValidation.HasValue,
                DateValidation = demande.DateValidation,
                MembreDirection = demande.MembreDirection != null ? new MinimalMembreDirectionDto
                {
                    Id = demande.MembreDirection.Id,
                    Nom = demande.MembreDirection.Nom,
                    Prenom = demande.MembreDirection.Prenom,
                    Fonction = demande.MembreDirection.Fonction
                } : null,
                Memoire = demande.Memoire != null ? new MemoireMinimalDto
                {
                    Id = demande.Memoire.Id,
                    Titre = demande.Memoire.Titre,
                    DateDepot = demande.Memoire.DateDepot
                } : null
            };
        }

        // GET: api/DemandeDepotMemoire/Encadreur/5
        [HttpGet("Encadreur/{encadreurId}")]
        [Authorize(Roles = "Direction,Encadreur")]
        public async Task<ActionResult<IEnumerable<DemandeDepotMemoireListDto>>> GetDemandesDepotMemoireByEncadreur(int encadreurId)
        {
            var demandes = await _context.DemandesDepotMemoire
                .Include(d => d.Theme)
                .Include(d => d.Stage)
                .Include(d => d.Encadreur)
                .Where(d => d.EncadreurId == encadreurId)
                .Select(d => new DemandeDepotMemoireListDto
                {
                    Id = d.Id,
                    DateDemande = d.DateDemande,
                    Statut = d.Statut,
                    NomTheme = d.Theme.Nom,
                    NomPrenomEtudiants = d.NomPrenomEtudiants,
                    NomPrenomEncadreur = d.NomPrenomEncadreur,
                    DateDebutStage = d.Stage.DateDebut,
                    DateFinStage = d.Stage.DateFin,
                    EstValideParDirection = d.DateValidation.HasValue
                })
                .ToListAsync();

            return demandes;
        }

        private bool DemandeDepotMemoireExists(int id)
        {
            return _context.DemandesDepotMemoire.Any(e => e.Id == id);
        }
    }

    public class ValidationDemandeDto
    {
        public bool EstApprouve { get; set; }
        public int MembreDirectionId { get; set; }
        public string Commentaire { get; set; }
    }
}
