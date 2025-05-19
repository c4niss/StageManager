using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.StageDTO;
using StageManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Stage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StageDto>>> GetStages()
        {
            var stages = await _context.Stages
                .Include(s => s.Encadreur)
                .Include(s => s.Departement)
                .Include(s => s.Stagiaires)
                .ToListAsync();
            return stages.Select(s => new StageDto
            {
                Id = s.Id,
                StagiaireGroup = s.StagiaireGroup,
                DateDebut = s.DateDebut,
                DateFin = s.DateFin,
                Statut = s.Statut,
                ConventionId = s.ConventionId,
                DepartementId = s.DepartementId,
                DepartementNom = s.Departement?.Nom,
                EncadreurId = s.EncadreurId,
                EncadreurNomComplet = $"{s.Encadreur?.Nom} {s.Encadreur?.Prenom}",
                Stagiaires = s.Stagiaires?.Select(st => new StagiaireInfoDto
                {
                    Id = st.Id,
                    NomComplet = $"{st.Nom} {st.Prenom}",
                    Email = st.Email
                }).ToList()
            }).ToList();
        }

        // GET: api/Stage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StageDto>> GetStage(int id)
        {
            var stage = await _context.Stages
                .Include(s => s.Encadreur)
                .Include(s => s.Departement)
                .Include(s => s.Stagiaires)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (stage == null)
            {
                return NotFound();
            }

            var stageDto = new StageDto
            {
                Id = stage.Id,
                StagiaireGroup = stage.StagiaireGroup,
                DateDebut = stage.DateDebut,
                DateFin = stage.DateFin,
                Statut = stage.Statut,
                ConventionId = stage.ConventionId,
                DepartementId = stage.DepartementId,
                DepartementNom = stage.Departement?.Nom,
                EncadreurId = stage.EncadreurId,
                EncadreurNomComplet = $"{stage.Encadreur?.Nom} {stage.Encadreur?.Prenom}",
                Stagiaires = stage.Stagiaires?.Select(st => new StagiaireInfoDto
                {
                    Id = st.Id,
                    NomComplet = $"{st.Nom} {st.Prenom}",
                    Email = st.Email
                }).ToList()
            };
            return stageDto;
        }

        // POST: api/Stage
        [HttpPost]
        public async Task<ActionResult<Stage>> CreateStage(CreateStageDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Vérifier que l'encadreur existe et est disponible
            var encadreur = await _context.Encadreurs.FindAsync(dto.EncadreurId);
            if (encadreur == null)
            {
                return BadRequest("L'encadreur spécifié n'existe pas.");
            }

            if (!encadreur.EstDisponible)
            {
                return BadRequest("L'encadreur n'est pas disponible ou a atteint son quota maximum de stagiaires.");
            }

            // Vérifier que le département existe
            if (!await _context.Departements.AnyAsync(d => d.Id == dto.DepartementId))
            {
                return BadRequest("Le département spécifié n'existe pas.");
            }

            // Vérifier que la convention existe
            if (!await _context.Conventions.AnyAsync(c => c.Id == dto.ConventionId))
            {
                return BadRequest("La convention spécifiée n'existe pas.");
            }

            // Créer le stage
            var stage = new Stage
            {
                StagiaireGroup = dto.StagiaireGroup,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin,
                Statut = StatutStage.EnCours,
                ConventionId = dto.ConventionId,
                DepartementId = dto.DepartementId,
                EncadreurId = dto.EncadreurId
            };

            _context.Stages.Add(stage);
            await _context.SaveChangesAsync();

            // Associer les stagiaires au stage
            if (dto.StagiaireIds != null && dto.StagiaireIds.Any())
            {
                var stagiaires = await _context.Stagiaires
                    .Where(s => dto.StagiaireIds.Contains(s.Id))
                    .ToListAsync();

                foreach (var stagiaire in stagiaires)
                {
                    stagiaire.StageId = stage.Id;
                }

                // Mettre à jour le nombre de stagiaires pour l'encadreur
                encadreur.NbrStagiaires += stagiaires.Count;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetStage), new { id = stage.Id }, stage);
        }

        // PUT: api/Stage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStage(int id, UpdateStageDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stage = await _context.Stages.FindAsync(id);
            if (stage == null)
            {
                return NotFound();
            }

            // Mettre à jour les propriétés si fournies
            if (dto.StagiaireGroup != null)
            {
                stage.StagiaireGroup = dto.StagiaireGroup;
            }

            if (dto.DateDebut.HasValue)
            {
                stage.DateDebut = dto.DateDebut.Value;
            }

            if (dto.DateFin.HasValue)
            {
                stage.DateFin = dto.DateFin.Value;
            }

            if (dto.Statut.HasValue)
            {
                stage.Statut = dto.Statut.Value;
            }

            // Gérer le changement d'encadreur si nécessaire
            if (dto.EncadreurId.HasValue && dto.EncadreurId.Value != stage.EncadreurId)
            {
                var oldEncadreur = await _context.Encadreurs.FindAsync(stage.EncadreurId);
                var newEncadreur = await _context.Encadreurs.FindAsync(dto.EncadreurId.Value);

                if (newEncadreur == null)
                {
                    return BadRequest("Le nouvel encadreur spécifié n'existe pas.");
                }

                if (!newEncadreur.EstDisponible)
                {
                    return BadRequest("Le nouvel encadreur n'est pas disponible.");
                }

                // Compter les stagiaires associés à ce stage
                var stagiaireCount = await _context.Stagiaires.CountAsync(s => s.StageId == id);

                // Mettre à jour les compteurs des encadreurs
                if (oldEncadreur != null)
                {
                    oldEncadreur.NbrStagiaires -= stagiaireCount;
                }

                newEncadreur.NbrStagiaires += stagiaireCount;


                stage.EncadreurId = dto.EncadreurId.Value;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StageExists(id))
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

        // PATCH: api/Stage/5/Status
        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> UpdateStageStatus(int id, StageStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stage = await _context.Stages.FindAsync(id);
            if (stage == null)
            {
                return NotFound();
            }

            // Valider la transition d'état
            if (dto.Statut == StatutStage.Termine && stage.Statut != StatutStage.EnCours && stage.Statut != StatutStage.Prolonge)
            {
                return BadRequest("Un stage ne peut être marqué comme terminé que s'il est en cours ou prolongé.");
            }

            // Enregistrer la raison pour certaines transitions
            var needsReason = dto.Statut == StatutStage.Annule || dto.Statut == StatutStage.Prolonge;
            if (needsReason && string.IsNullOrEmpty(dto.Raison))
            {
                return BadRequest("Une raison est requise pour annuler ou prolonger un stage.");
            }

            stage.Statut = dto.Statut;

            // Si le stage est terminé ou annulé, libérer l'encadreur
            if (dto.Statut == StatutStage.Termine || dto.Statut == StatutStage.Annule)
            {
                var encadreur = await _context.Encadreurs.FindAsync(stage.EncadreurId);
                if (encadreur != null)
                {
                    var stagiaireCount = await _context.Stagiaires.CountAsync(s => s.StageId == id);
                    encadreur.NbrStagiaires -= stagiaireCount;

                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Stage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStage(int id)
        {
            var stage = await _context.Stages
                .Include(s => s.Stagiaires)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stage == null)
            {
                return NotFound();
            }

            // Mettre à jour les stagiaires pour qu'ils ne soient plus liés à ce stage
            if (stage.Stagiaires != null)
            {
                foreach (var stagiaire in stage.Stagiaires)
                {
                    stagiaire.StageId = null;
                }
            }

            // Mettre à jour le compteur de l'encadreur
            var encadreur = await _context.Encadreurs.FindAsync(stage.EncadreurId);
            if (encadreur != null)
            {
                var stagiaireCount = stage.Stagiaires?.Count ?? 0;
                encadreur.NbrStagiaires -= stagiaireCount;
            }

            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Stage/Departement/5
        [HttpGet("Departement/{departementId}")]
        public async Task<ActionResult<IEnumerable<StageDto>>> GetStagesByDepartement(int departementId)
        {
            var stages = await _context.Stages
                .Include(s => s.Encadreur)
                .Include(s => s.Departement)
                .Include(s => s.Stagiaires)
                .Where(s => s.DepartementId == departementId)
                .ToListAsync();

            if (!stages.Any())
            {
                return new List<StageDto>();
            }

            return stages.Select(s => new StageDto
            {
                Id = s.Id,
                StagiaireGroup = s.StagiaireGroup,
                DateDebut = s.DateDebut,
                DateFin = s.DateFin,
                Statut = s.Statut,
                ConventionId = s.ConventionId,
                DepartementId = s.DepartementId,
                DepartementNom = s.Departement?.Nom,
                EncadreurId = s.EncadreurId,
                EncadreurNomComplet = $"{s.Encadreur?.Nom} {s.Encadreur?.Prenom}",
                Stagiaires = s.Stagiaires?.Select(st => new StagiaireInfoDto
                {
                    Id = st.Id,
                    NomComplet = $"{st.Nom} {st.Prenom}",
                    Email = st.Email
                }).ToList()
            }).ToList();
        }
        // GET: api/Stage/Search?query=informatique
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<StageDto>>> SearchStages(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Le terme de recherche ne peut pas être vide.");
            }

            // Normaliser la requête pour la recherche
            query = query.ToLower().Trim();

            var stages = await _context.Stages
                .Include(s => s.Encadreur)
                .Include(s => s.Departement)
                .Include(s => s.Stagiaires)
                .Include(s => s.Convention)
                    .ThenInclude(c => c.DemandeAccord)
                        .ThenInclude(da => da.Theme)
                .Where(s =>
                    // Recherche dans le groupe de stagiaires
                    s.StagiaireGroup.ToLower().Contains(query) ||
                    // Recherche dans le nom du département
                    s.Departement.Nom.ToLower().Contains(query) ||
                    // Recherche dans le nom/prénom de l'encadreur
                    (s.Encadreur.Nom + " " + s.Encadreur.Prenom).ToLower().Contains(query) ||
                    // Recherche dans le thème associé au stage via la convention et la demande d'accord
                    (s.Convention != null && s.Convention.DemandeAccord != null &&
                     s.Convention.DemandeAccord.Theme != null &&
                     s.Convention.DemandeAccord.Theme.Nom.ToLower().Contains(query)) ||
                    // Recherche dans les noms des stagiaires
                    s.Stagiaires.Any(st => (st.Nom + " " + st.Prenom).ToLower().Contains(query) ||
                                           st.Email.ToLower().Contains(query))
                )
                .ToListAsync();

            if (!stages.Any())
            {
                return new List<StageDto>();
            }

            return stages.Select(s => new StageDto
            {
                Id = s.Id,
                StagiaireGroup = s.StagiaireGroup,
                DateDebut = s.DateDebut,
                DateFin = s.DateFin,
                Statut = s.Statut,
                ConventionId = s.ConventionId,
                DepartementId = s.DepartementId,
                DepartementNom = s.Departement?.Nom,
                EncadreurId = s.EncadreurId,
                EncadreurNomComplet = $"{s.Encadreur?.Nom} {s.Encadreur?.Prenom}",
                Stagiaires = s.Stagiaires?.Select(st => new StagiaireInfoDto
                {
                    Id = st.Id,
                    NomComplet = $"{st.Nom} {st.Prenom}",
                    Email = st.Email
                }).ToList()
            }).ToList();
        }

        // GET: api/Stage/Encadreur/5
        [HttpGet("Encadreur/{encadreurId}")]
        public async Task<ActionResult<IEnumerable<StageDto>>> GetStagesByEncadreur(int encadreurId)
        {
            var stages = await _context.Stages
                .Include(s => s.Encadreur)
                .Include(s => s.Departement)
                .Include(s => s.Stagiaires)
                .Where(s => s.EncadreurId == encadreurId)
                .ToListAsync();

            if (!stages.Any())
            {
                return new List<StageDto>();
            }

            return stages.Select(s => new StageDto
            {
                Id = s.Id,
                StagiaireGroup = s.StagiaireGroup,
                DateDebut = s.DateDebut,
                DateFin = s.DateFin,
                Statut = s.Statut,
                ConventionId = s.ConventionId,
                DepartementId = s.DepartementId,
                DepartementNom = s.Departement?.Nom,
                EncadreurId = s.EncadreurId,
                EncadreurNomComplet = $"{s.Encadreur?.Nom} {s.Encadreur?.Prenom}",
                Stagiaires = s.Stagiaires?.Select(st => new StagiaireInfoDto
                {
                    Id = st.Id,
                    NomComplet = $"{st.Nom} {st.Prenom}",
                    Email = st.Email
                }).ToList()
            }).ToList();
        }

        private bool StageExists(int id)
        {
            return _context.Stages.Any(e => e.Id == id);
        }
    }
}