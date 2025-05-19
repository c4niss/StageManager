using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO;
using StageManager.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "MembreDirection")]
    public class MembreDirectionStatistiquesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MembreDirectionStatistiquesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<MembreDirectionStatistiquesDto>> GetStatistiques()
        {
            // Récupérer l'ID du membre de direction connecté
            var membreDirectionId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Récupérer le membre de direction
            var membreDirection = await _context.MembresDirection
                .FirstOrDefaultAsync(m => m.Id == membreDirectionId);

            if (membreDirection == null)
            {
                return NotFound("Membre de direction non trouvé");
            }

            // Statistiques des demandes de stage
            var demandesDeStage = await _context.DemandesDeStage
                .Where(d => d.MembreDirectionId == membreDirectionId)
                .ToListAsync();

            var demandeDeStageStats = new DemandeDeStageStats
            {
                Total = demandesDeStage.Count,
                EnAttente = demandesDeStage.Count(d => d.Statut == DemandeDeStage.StatusDemandeDeStage.Enattente),
                Acceptees = demandesDeStage.Count(d => d.Statut == DemandeDeStage.StatusDemandeDeStage.Accepte),
                Refusees = demandesDeStage.Count(d => d.Statut == DemandeDeStage.StatusDemandeDeStage.Refuse)
            };

            // Récupérer les demandes d'accord liées aux demandes de stage
            var demandeStageIds = demandesDeStage.Select(d => d.Id).ToList();
            var demandesAccord = await _context.DemandesAccord
                .Where(da => demandeStageIds.Contains(da.DemandeStageId))
                .ToListAsync();

            var demandeAccordStats = new DemandeAccord1Stats
            {
                Total = demandesAccord.Count,
                EnAttente = demandesAccord.Count(d => d.Status == StatusAccord.EnAttente),
                EnCours = demandesAccord.Count(d => d.Status == StatusAccord.EnCours),
                Acceptees = demandesAccord.Count(d => d.Status == StatusAccord.Accepte),
                Refusees = demandesAccord.Count(d => d.Status == StatusAccord.Refuse)
            };

            // Statistiques des conventions
            var conventions = await _context.Conventions
                .Where(c => c.MembreDirectionId == membreDirectionId)
                .ToListAsync();

            var conventionStats = new ConventionStats
            {
                Total = conventions.Count,
                EnAttente = conventions.Count(c => c.status == Convention.Statusconvention.EnAttente),
                Acceptees = conventions.Count(c => c.status == Convention.Statusconvention.Accepte),
                Refusees = conventions.Count(c => c.status == Convention.Statusconvention.Refuse)
            };

            // 1. Récupérer les stages liés aux conventions
            var conventionIds = conventions.Select(c => c.Id).ToList();
            var stagesFromConventions = await _context.Stages
                .Where(s => s.ConventionId.HasValue && conventionIds.Contains(s.ConventionId.Value))
                .ToListAsync();

            // 2. Récupérer les stages liés aux demandes d'accord
            var demandeAccordIds = demandesAccord.Select(da => da.Id).ToList();
            var stagesFromDemandesAccord = await _context.Stages
                .Include(s => s.Convention)
                .Where(s => s.Convention != null && demandeAccordIds.Contains(s.Convention.DemandeAccordId))
                .ToListAsync();

            // 3. Fusionner les deux listes de stages (en évitant les doublons)
            var allStages = stagesFromConventions
                .Union(stagesFromDemandesAccord, new StageIdComparer())
                .ToList();

            var stageStats = new StageStats
            {
                Total = allStages.Count,
                EnCours = allStages.Count(s => s.Statut == StatutStage.EnAttente),
                EnAttente = allStages.Count(s => s.Statut == StatutStage.EnAttente),
                Termines = allStages.Count(s => s.Statut == StatutStage.Termine),
                Annules = allStages.Count(s => s.Statut == StatutStage.Annule),
                Prolonges = allStages.Count(s => s.Statut == StatutStage.Prolonge)
            };

            // Statistiques des attestations
            var attestations = await _context.Attestations
                .Where(a => a.MembreDirectionId == membreDirectionId)
                .ToListAsync();

            var attestationStats = new AttestationStats
            {
                Total = attestations.Count,
                Delivrees = attestations.Count(a => a.EstDelivree),
                NonDelivrees = attestations.Count(a => !a.EstDelivree)
            };

            // Nombre total de stagiaires (à partir des stages et des demandes de stage)
            var allStageIds = allStages.Select(s => s.Id).ToList();
            var stagiairesFromStages = await _context.Stagiaires
                .Where(s => s.StageId.HasValue && allStageIds.Contains(s.StageId.Value))
                .ToListAsync();
            var stagiairesFromDemandes = await _context.Stagiaires
                .Where(s => s.DemandeDeStageId.HasValue && demandeStageIds.Contains(s.DemandeDeStageId.Value))
                .ToListAsync();

            // Éviter de compter deux fois les mêmes stagiaires
            var allStagiaires = stagiairesFromStages
                .Union(stagiairesFromDemandes, new StagiaireIdComparer())
                .ToList();

            // Construire l'objet de réponse
            return new MembreDirectionStatistiquesDto
            {
                DemandesDeStage = demandeDeStageStats,
                DemandesAccord = demandeAccordStats,
                Conventions = conventionStats,
                Stages = stageStats,
                Attestations = attestationStats,
                TotalStagiaires = allStagiaires.Count
            };
        }
    }

    // Classes pour éviter les doublons lors de l'union des listes
    public class StageIdComparer : IEqualityComparer<Stage>
    {
        public bool Equals(Stage x, Stage y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Stage obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class StagiaireIdComparer : IEqualityComparer<Stagiaire>
    {
        public bool Equals(Stagiaire x, Stagiaire y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Stagiaire obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}