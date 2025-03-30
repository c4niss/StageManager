
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ChefDepartementDTO;
using StageManager.Models;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefDepartementController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChefDepartementController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ChefDepartement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChefDepartementDto>>> GetChefDepartements()
        {
            var chefDeps = await _context.ChefDepartements
                .Include(c => c.Departement)
                .ToListAsync();

            return chefDeps.Select(c => new ChefDepartementDto
            {
                Id = c.Id,
                Nom = c.Nom,
                Prenom = c.Prenom,
                Email = c.Email,
                DepartementId = c.DepartementId,
                DepartementNom = c.Departement?.Nom
            }).ToList();
        }

        // GET: api/ChefDepartement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChefDepartementDto>> GetChefDepartement(int id)
        {
            var chefDepartement = await _context.ChefDepartements
                .Include(c => c.Departement)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chefDepartement == null)
            {
                return NotFound();
            }

            return new ChefDepartementDto
            {
                Id = chefDepartement.Id,
                Nom = chefDepartement.Nom,
                Prenom = chefDepartement.Prenom,
                Email = chefDepartement.Email,
                DepartementId = chefDepartement.DepartementId,
                DepartementNom = chefDepartement.Departement?.Nom
            };
        }

        // POST: api/ChefDepartement/assign
        [HttpPost("assign")]
        public async Task<ActionResult<ChefDepartementDto>> AssignChefDepartement(AssignChefDepartementDto dto)
        {
            // Vérifier si le département existe
            var departement = await _context.Departements.FindAsync(dto.DepartementId);
            if (departement == null)
            {
                return BadRequest("Département non trouvé");
            }

            // Vérifier si l'encadreur existe
            var encadreur = await _context.Encadreurs.FindAsync(dto.EncadreurId);
            if (encadreur == null)
            {
                return BadRequest("Encadreur non trouvé");
            }

            // Vérifier si le département a déjà un chef
            if (departement.ChefDepartementId.HasValue)
            {
                // Optionnel: Récupérer l'ancien chef pour le redevenir encadreur simple
                var ancienChef = await _context.ChefDepartements.FindAsync(departement.ChefDepartementId);
                if (ancienChef != null)
                {
                    // Logique pour retransformer le chef en encadreur
                    // ...
                }
            }

            // Créer le nouveau chef de département à partir de l'encadreur
            var chefDepartement = new ChefDepartement
            {
                Nom = encadreur.Nom,
                Prenom = encadreur.Prenom,
                Email = encadreur.Email,
                Telephone = encadreur.Telephone,
                MotDePasse = encadreur.MotDePasse, // idéalement, à gérer plus proprement
                Role = "ChefDepartement",
                EstActif = true,
                PhotoUrl = encadreur.PhotoUrl,
                DepartementId = dto.DepartementId
            };

            // Commencer une transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Ajouter le chef et supprimer l'encadreur
                _context.ChefDepartements.Add(chefDepartement);
                _context.Encadreurs.Remove(encadreur);

                await _context.SaveChangesAsync();

                // Mettre à jour la référence du département vers le nouveau chef
                departement.ChefDepartementId = chefDepartement.Id;
                _context.Departements.Update(departement);

                await _context.SaveChangesAsync();

                // Valider la transaction
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // Annuler la transaction en cas d'erreur
                await transaction.RollbackAsync();
                throw;
            }

            // Retourner le chef nouvellement créé
            return new ChefDepartementDto
            {
                Id = chefDepartement.Id,
                Nom = chefDepartement.Nom,
                Prenom = chefDepartement.Prenom,
                Email = chefDepartement.Email,
                DepartementId = chefDepartement.DepartementId,
                DepartementNom = departement.Nom
            };
        }
    }
}