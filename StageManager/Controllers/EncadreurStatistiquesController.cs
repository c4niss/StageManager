using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO;
using StageManager.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Encadreur")]
    public class EncadreurStatistiquesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EncadreurStatistiquesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<EncadreurStatistiquesDto>> GetStatistiques()
        {
            // Récupérer l'ID de l'encadreur connecté
            var encadreurId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Récupérer l'encadreur
            var encadreur = await _context.Encadreurs
                .FirstOrDefaultAsync(e => e.Id == encadreurId);

            if (encadreur == null)
            {
                return NotFound("Encadreur non trouvé");
            }

            // Récupérer les stages associés à l'encadreur
            var stages = await _context.Stages
                .Where(s => s.EncadreurId == encadreurId)
                .ToListAsync();

            // Récupérer les stagiaires de ces stages
            var stageIds = stages.Select(s => s.Id).ToList();
            var stagiaires = await _context.Stagiaires
                .Where(s => s.StageId.HasValue && stageIds.Contains(s.StageId.Value))
                .ToListAsync();

            // Calculer les statistiques
            var totalStagiaires = stagiaires.Count;
            var stagiairesActifs = stagiaires.Count(s => s.EstActif);

            var stageStats = new StageStats
            {
                Total = stages.Count,
                EnCours = stages.Count(s => s.Statut == StatutStage.EnCours),
                EnAttente = stages.Count(s => s.Statut == StatutStage.EnAttente),
                Termines = stages.Count(s => s.Statut == StatutStage.Termine),
                Annules = stages.Count(s => s.Statut == StatutStage.Annule),
                Prolonges = stages.Count(s => s.Statut == StatutStage.Prolonge)
            };

            return new EncadreurStatistiquesDto
            {
                TotalStagiaires = totalStagiaires,
                StagiairesActifs = stagiairesActifs,
                Stages = stageStats,
                EstDisponible = encadreur.EstDisponible
            };
        }
    }
}