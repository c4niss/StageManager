using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StageManager.Models;
using StageManager.DTOs;
using StageManager.DTO;
using StageManager.DTO.JourPresenceDTO;
using StageManager.DTO.PointageMoisDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using TestRestApi.Data;
using System.ComponentModel.DataAnnotations;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FichesPointagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FichesPointagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FichesPointages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FichePointageListDto>>> GetFichesPointage()
        {
            var fichesPointage = await _context.FichesDePointage.ToListAsync();
            return fichesPointage.Select(f => new FichePointageListDto
            {
                Id = f.Id,
                NomPrenomStagiaire = f.NomPrenomStagiaire,
                StructureAccueil = f.StructureAccueil,
                DateDebutStage = f.DateDebutStage,
                DateFinStage = f.DateFinStage,
                NatureStage = f.NatureStage,
                EstComplete = !string.IsNullOrEmpty(f.DonneesPointage),
                EstValide = f.EstValide
            }).ToList();
        }

        // GET: api/FichesPointages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FichePointageReadDto>> GetFichePointage(int id)
        {
            var fichePointage = await _context.FichesDePointage
                .Include(f => f.Stagiaire)
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .Include(f => f.PointageMois)
                    .ThenInclude(p => p.JoursPresence)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fichePointage == null)
            {
                return NotFound();
            }

            return new FichePointageReadDto
            {
                Id = fichePointage.Id,
                DateCreation = fichePointage.DateCreation,
                NomPrenomStagiaire = fichePointage.NomPrenomStagiaire,
                StructureAccueil = fichePointage.StructureAccueil,
                NomQualitePersonneChargeSuivi = fichePointage.NomQualitePersonneChargeSuivi,
                DateDebutStage = fichePointage.DateDebutStage,
                DateFinStage = fichePointage.DateFinStage,
                NatureStage = fichePointage.NatureStage,
                DonneesPointage = fichePointage.DonneesPointage,
                Stagiaire = new StagiaireMinimalDto
                {
                    Id = fichePointage.Stagiaire.Id,
                    NomPrenom = $"{fichePointage.Stagiaire.Nom} {fichePointage.Stagiaire.Prenom}"
                },
                Encadreur = new EncadreurMinimalDto
                {
                    Id = fichePointage.Encadreur.Id,
                    NomPrenom = $"{fichePointage.Encadreur.Nom} {fichePointage.Encadreur.Prenom}"
                },
                Stage = new StageMinimalDto
                {
                    Id = fichePointage.Stage.Id
                }
            };
        }

      
        // PUT: api/FichesPointages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFichePointage(int id, FichePointageUpdateDto fichePointageDto)
        {
            var fichePointage = await _context.FichesDePointage
                .Include(f => f.PointageMois)
                .ThenInclude(p => p.JoursPresence)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fichePointage == null)
                return NotFound();

            // Vérification de la durée du stage selon sa nature
            TimeSpan dureeStage = fichePointageDto.DateFinStage - fichePointageDto.DateDebutStage;
            if (fichePointageDto.NatureStage == NatureStage.StageImpregnation && dureeStage.TotalDays > 31)
                return BadRequest("La durée d'un stage d'imprégnation ne doit pas dépasser un (1) mois");
            if (fichePointageDto.NatureStage == NatureStage.StageFinEtude && dureeStage.TotalDays > 183)
                return BadRequest("La durée d'un stage de mémoire ne doit pas dépasser six (6) mois");

            // Mise à jour des propriétés de base
            fichePointage.NomPrenomStagiaire = fichePointageDto.NomPrenomStagiaire;
            fichePointage.StructureAccueil = fichePointageDto.StructureAccueil;
            fichePointage.NomQualitePersonneChargeSuivi = fichePointageDto.NomQualitePersonneChargeSuivi;
            fichePointage.DateDebutStage = fichePointageDto.DateDebutStage;
            fichePointage.DateFinStage = fichePointageDto.DateFinStage;
            fichePointage.NatureStage = fichePointageDto.NatureStage;
            if (!string.IsNullOrEmpty(fichePointageDto.DonneesPointage))
                fichePointage.DonneesPointage = fichePointageDto.DonneesPointage;

            // --- GESTION DES MOIS ET JOURS ---
            if (fichePointageDto.PointageMois != null && fichePointageDto.PointageMois.Any())
            {
                // Supprimer tous les anciens mois et jours associés
                foreach (var mois in fichePointage.PointageMois.ToList())
                {
                    _context.jourPresences.RemoveRange(mois.JoursPresence);
                    _context.PointageMois.Remove(mois);
                }

                // Ajouter les nouveaux mois et jours depuis le DTO
                foreach (var moisDto in fichePointageDto.PointageMois)
                {
                    var nouveauMois = new PointageMois
                    {
                        Mois = moisDto.Mois,
                        Annee = moisDto.Annee,
                        FicheDePointageId = fichePointage.Id,
                        JoursPresence = new List<JourPresence>()
                    };
                    _context.PointageMois.Add(nouveauMois);
                    await _context.SaveChangesAsync();

                    if (moisDto.JoursPresence != null)
                    {
                        foreach (var jourDto in moisDto.JoursPresence)
                        {
                            var nouveauJour = new JourPresence
                            {
                                Jour = jourDto.Jour,
                                JourSemaine = jourDto.JourSemaine,
                                EstPresent = jourDto.EstPresent,
                                Commentaire = jourDto.Commentaire,
                                PointageMoisId = nouveauMois.Id
                            };
                            _context.jourPresences.Add(nouveauJour);
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichePointageExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }


        // GET: api/FichesPointages/{id}/PointageMois
        [HttpGet("{id}/PointageMois")]
        public async Task<ActionResult<IEnumerable<PointageMoisDto>>> GetPointageMois(int id)
        {
            var fichePointage = await _context.FichesDePointage
                .Include(f => f.PointageMois)
                    .ThenInclude(p => p.JoursPresence)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fichePointage == null)
            {
                return NotFound();
            }

            var pointageMoisDtos = fichePointage.PointageMois.Select(p => new PointageMoisDto
            {
                Id = p.Id,
                Mois = p.Mois,
                Annee = p.Annee,
                JoursPresence = p.JoursPresence.Select(j => new JourPresenceDto
                {
                    Id = j.Id,
                    Jour = j.Jour,
                    JourSemaine = j.JourSemaine,
                    EstPresent = j.EstPresent,
                    Commentaire = j.Commentaire
                }).ToList()
            }).ToList();

            return pointageMoisDtos;
        }

        // POST: api/FichesPointages/Validate
        [HttpPost("Validate")]
        public async Task<IActionResult> ValidateFichePointage(int id, FichePointageValidationDto validationDto)
        {
            var fichePointage = await _context.FichesDePointage.FindAsync(id);
            if (fichePointage == null)
            {
                return NotFound();
            }

            // Vérifier que la fiche de pointage a des données
            if (string.IsNullOrEmpty(fichePointage.DonneesPointage))
            {
                return BadRequest("La fiche de pointage ne contient pas de données à valider");
            }

            fichePointage.EstValide = validationDto.EstValide;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichePointageExists(id))
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

        // DELETE: api/FichesPointages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFichePointage(int id)
        {
            var fichePointage = await _context.FichesDePointage
                .Include(f => f.PointageMois)
                    .ThenInclude(p => p.JoursPresence)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fichePointage == null)
            {
                return NotFound();
            }

            // Supprimer d'abord les jours de présence et les mois de pointage
            foreach (var mois in fichePointage.PointageMois)
            {
                _context.jourPresences.RemoveRange(mois.JoursPresence);
                _context.PointageMois.Remove(mois);
            }

            _context.FichesDePointage.Remove(fichePointage);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/FichesPointages/Stagiaire/5
        [HttpGet("Stagiaire/{stagiaireId}")]
        public async Task<ActionResult<IEnumerable<FichePointageListDto>>> GetFichesPointageByStagiaire(int stagiaireId)
        {
            var fichesPointage = await _context.FichesDePointage
                .Where(f => f.StagiaireId == stagiaireId)
                .ToListAsync();

            return fichesPointage.Select(f => new FichePointageListDto
            {
                Id = f.Id,
                NomPrenomStagiaire = f.NomPrenomStagiaire,
                StructureAccueil = f.StructureAccueil,
                DateDebutStage = f.DateDebutStage,
                DateFinStage = f.DateFinStage,
                NatureStage = f.NatureStage,
                EstComplete = !string.IsNullOrEmpty(f.DonneesPointage),
                EstValide = f.EstValide
            }).ToList();
        }

        // GET: api/FichesPointages/Encadreur/5
        [HttpGet("Encadreur/{encadreurId}")]
        public async Task<ActionResult<IEnumerable<FichePointageListDto>>> GetFichesPointageByEncadreur(int encadreurId)
        {
            var fichesPointage = await _context.FichesDePointage
                .Where(f => f.EncadreurId == encadreurId)
                .ToListAsync();

            return fichesPointage.Select(f => new FichePointageListDto
            {
                Id = f.Id,
                NomPrenomStagiaire = f.NomPrenomStagiaire,
                StructureAccueil = f.StructureAccueil,
                DateDebutStage = f.DateDebutStage,
                DateFinStage = f.DateFinStage,
                NatureStage = f.NatureStage,
                EstComplete = !string.IsNullOrEmpty(f.DonneesPointage),
                EstValide = f.EstValide
            }).ToList();
        }

        private bool FichePointageExists(int id)
        {
            return _context.FichesDePointage.Any(e => e.Id == id);
        }
    }

    public class DonneesPointageUpdateDto
    {
        [Required(ErrorMessage = "L'identifiant de la fiche de pointage est obligatoire")]
        public int FichePointageId { get; set; }

        [Required(ErrorMessage = "Les données de pointage sont obligatoires")]
        public string DonneesPointage { get; set; }
    }
}