using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatistiquesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatistiquesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StatistiquesDto>> GetStatistiques()
        {
            // Statistiques pour MembreDirection
            var totalMembresDirection = await _context.MembresDirection.CountAsync();
            var membresDirectionActifs = await _context.MembresDirection.CountAsync(m => m.EstActif);

            // Statistiques pour Encadreur
            var totalEncadreurs = await _context.Encadreurs.CountAsync();
            var encadreursActifs = await _context.Encadreurs.CountAsync(e => e.EstActif);

            // Statistiques pour ChefDepartement
            var totalChefDepartements = await _context.ChefDepartements.CountAsync();
            var chefDepartementsActifs = await _context.ChefDepartements.CountAsync(c => c.EstActif);

            // Statistiques pour Stagiaire
            var totalStagiaires = await _context.Stagiaires.CountAsync();
            var stagiairesActifs = await _context.Stagiaires.CountAsync(s => s.EstActif);

            // Calculer les totaux généraux
            var totalUtilisateurs = totalMembresDirection + totalEncadreurs + totalChefDepartements + totalStagiaires;
            var totalUtilisateursActifs = membresDirectionActifs + encadreursActifs + chefDepartementsActifs + stagiairesActifs;

            return new StatistiquesDto
            {
                MembreDirection = new UserTypeStats
                {
                    Total = totalMembresDirection,
                    Actifs = membresDirectionActifs
                },
                Encadreur = new UserTypeStats
                {
                    Total = totalEncadreurs,
                    Actifs = encadreursActifs
                },
                ChefDepartement = new UserTypeStats
                {
                    Total = totalChefDepartements,
                    Actifs = chefDepartementsActifs
                },
                Stagiaire = new UserTypeStats
                {
                    Total = totalStagiaires,
                    Actifs = stagiairesActifs
                },
                TotalUtilisateurs = totalUtilisateurs,
                TotalUtilisateursActifs = totalUtilisateursActifs
            };
        }

        // Endpoint pour obtenir les statistiques d'un type d'utilisateur spécifique
        [HttpGet("{type}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserTypeStats>> GetStatistiquesByType(string type)
        {
            switch (type.ToLower())
            {
                case "membredirection":
                    return new UserTypeStats
                    {
                        Total = await _context.MembresDirection.CountAsync(),
                        Actifs = await _context.MembresDirection.CountAsync(m => m.EstActif)
                    };
                case "encadreur":
                    return new UserTypeStats
                    {
                        Total = await _context.Encadreurs.CountAsync(),
                        Actifs = await _context.Encadreurs.CountAsync(e => e.EstActif)
                    };
                case "chefdepartement":
                    return new UserTypeStats
                    {
                        Total = await _context.ChefDepartements.CountAsync(),
                        Actifs = await _context.ChefDepartements.CountAsync(c => c.EstActif)
                    };
                case "stagiaire":
                    return new UserTypeStats
                    {
                        Total = await _context.Stagiaires.CountAsync(),
                        Actifs = await _context.Stagiaires.CountAsync(s => s.EstActif)
                    };
                default:
                    return BadRequest("Type d'utilisateur non reconnu. Utilisez 'membredirection', 'encadreur', 'chefdepartement' ou 'stagiaire'.");
            }
        }

        // Nouvel endpoint pour obtenir uniquement le nombre total d'utilisateurs
        [HttpGet("total")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TotalUtilisateursDto>> GetTotalUtilisateurs()
        {
            // Compter tous les utilisateurs
            var totalMembresDirection = await _context.MembresDirection.CountAsync();
            var totalEncadreurs = await _context.Encadreurs.CountAsync();
            var totalChefDepartements = await _context.ChefDepartements.CountAsync();
            var totalStagiaires = await _context.Stagiaires.CountAsync();

            // Compter les utilisateurs actifs
            var membresDirectionActifs = await _context.MembresDirection.CountAsync(m => m.EstActif);
            var encadreursActifs = await _context.Encadreurs.CountAsync(e => e.EstActif);
            var chefDepartementsActifs = await _context.ChefDepartements.CountAsync(c => c.EstActif);
            var stagiairesActifs = await _context.Stagiaires.CountAsync(s => s.EstActif);

            var total = totalMembresDirection + totalEncadreurs + totalChefDepartements + totalStagiaires;
            var totalActifs = membresDirectionActifs + encadreursActifs + chefDepartementsActifs + stagiairesActifs;

            return new TotalUtilisateursDto
            {
                Total = total,
                Actifs = totalActifs
            };
        }
    }
}