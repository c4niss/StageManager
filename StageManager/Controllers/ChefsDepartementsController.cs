using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ChefDepartementDTO;
using StageManager.Models;
using System.Threading.Tasks;
using TestRestApi.Data;
using System.Collections.Generic;
using System.Linq;
using System;
using StageManager.Mapping;

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

            return chefDeps.Select(c => ChefDepartementMapping.ToDto(c)).ToList();
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

            return ChefDepartementMapping.ToDto(chefDepartement);
        }
        [HttpPost]
        public async Task<IActionResult> CreateChefDepartement(CreateChefDepartementDto newchef)
        {
            var existingusername = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Username == newchef.Username);
            if (existingusername != null)
            {
                return BadRequest("Le nom d'utilisateur existe déjà.");
            }
                var chefdepartement = new ChefDepartement
            {
                Nom = newchef.Nom,
                Prenom = newchef.Prenom,
                Email = newchef.Email,
                Username = newchef.Username,
                Telephone = newchef.Telephone,
                MotDePasse = newchef.MotDePasse,
                EstActif = true,
                PhotoUrl = newchef.PhotoUrl
            };
            _context.ChefDepartements.Add(chefdepartement);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetChefDepartement), new { id = chefdepartement.Id }, ChefDepartementMapping.ToDto(chefdepartement));
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
                    // Convertir l'ancien chef en encadreur
                    var nouvelEncadreur = new Encadreur
                    {
                        Nom = ancienChef.Nom,
                        Prenom = ancienChef.Prenom,
                        Email = ancienChef.Email,
                        Telephone = ancienChef.Telephone,
                        MotDePasse = ancienChef.MotDePasse,
                        Role = "Encadreur",
                        EstActif = true,
                        PhotoUrl = ancienChef.PhotoUrl,
                        DepartementId = ancienChef.DepartementId,
                        EstDisponible = true,
                        StagiaireMax = 5
                    };

                    _context.Encadreurs.Add(nouvelEncadreur);
                    _context.ChefDepartements.Remove(ancienChef);
                }
            }

            // Créer le nouveau chef de département à partir de l'encadreur
            var chefDepartement = new ChefDepartement
            {
                Nom = encadreur.Nom,
                Prenom = encadreur.Prenom,
                Email = encadreur.Email,
                Telephone = encadreur.Telephone,
                MotDePasse = encadreur.MotDePasse,
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

            return ChefDepartementMapping.ToDto(chefDepartement);
        }
    }
}