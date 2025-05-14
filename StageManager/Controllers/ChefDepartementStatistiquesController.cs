using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO;
using StageManager.Models;
using System.Linq;
using System.Security.Claims;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ChefDepartement")]
    public class ChefDepartementStatistiquesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChefDepartementStatistiquesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ChefDepartementStatistiquesDto>> GetStatistiques()
        {
            // Récupérer l'ID du chef de département connecté
            var chefId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Récupérer le chef de département
            var chef = await _context.ChefDepartements
                .Include(c => c.Departement)
                .FirstOrDefaultAsync(c => c.Id == chefId);

            if (chef == null)
            {
                return NotFound("Chef de département non trouvé");
            }

            // Récupérer les encadreurs du département
            var encadreurs = await _context.Encadreurs
                .Where(e => e.DepartementId == chef.DepartementId)
                .ToListAsync();

            var encadreurIds = encadreurs.Select(e => e.Id).ToList();

            // Récupérer les stages associés au département
            var stages = await _context.Stages
                .Where(s => encadreurIds.Contains((int)s.EncadreurId))
                .ToListAsync();

            // Récupérer les stagiaires de ces stages
            var stageIds = stages.Select(s => s.Id).ToList();
            var stagiaires = await _context.Stagiaires
                .Where(s => s.StageId.HasValue && stageIds.Contains(s.StageId.Value))
                .ToListAsync();

            // Récupérer les demandes d'accord traitées par ce chef
            var demandesAccord = await _context.DemandesAccord
                .Where(d => d.ChefDepartementId == chefId)
                .ToListAsync();

            // Calculer les statistiques
            var totalStagiaires = stagiaires.Count;
            var stagiairesActifs = stagiaires.Count(s => s.EstActif);
            var encadreursDisponibles = encadreurs.Count(e => e.EstDisponible);

            var stageStats = new StageStats
            {
                Total = stages.Count,
                EnCours = stages.Count(s => s.Statut == StatutStage.EnCours),
                EnAttente = stages.Count(s => s.Statut == StatutStage.EnAttente),
                Termines = stages.Count(s => s.Statut == StatutStage.Termine),
                Annules = stages.Count(s => s.Statut == StatutStage.Annule),
                Prolonges = stages.Count(s => s.Statut == StatutStage.Prolonge)
            };

            var demandeAccordStats = new DemandeAccordStats
            {
                Total = demandesAccord.Count,
                EnAttente = demandesAccord.Count(d => d.Status == StatusAccord.EnAttente),
                Approuvees = demandesAccord.Count(d => d.Status == StatusAccord.Accepte),
                Rejetees = demandesAccord.Count(d => d.Status == StatusAccord.Refuse)
            };

            return new ChefDepartementStatistiquesDto
            {
                TotalEncadreurs = encadreurs.Count,
                EncadreursDisponibles = encadreursDisponibles,
                TotalStagiaires = totalStagiaires,
                StagiairesActifs = stagiairesActifs,
                Stages = stageStats,
                TotalDemandesAccord = demandesAccord.Count,
                DemandesAccord = demandeAccordStats,
                NomDepartement = chef.Departement?.Nom ?? "Non spécifié"
            };
        }
    }
}
