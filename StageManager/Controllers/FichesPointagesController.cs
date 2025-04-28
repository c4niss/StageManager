using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StageManager.Models;
using StageManager.DTOs;
using StageManager.DTO;
using StageManager.Mapping;
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

        [HttpGet]
        [Authorize(Roles = "Direction,Encadreur")]
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

        // GET: api/FichePointage/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Direction,Encadreur,Stagiaire")]
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

        // POST: api/FichePointage
        [HttpPost]
        public async Task<ActionResult<FichePointageReadDto>> CreateFichePointage(FichePointageCreateDto fichePointageDto)
        {
            // Vérifier si le stagiaire, l'encadreur et le stage existent
            var stagiaire = await _context.Stagiaires.FindAsync(fichePointageDto.StagiaireId);
            var encadreur = await _context.Encadreurs.FindAsync(fichePointageDto.EncadreurId);
            var stage = await _context.Stages.FindAsync(fichePointageDto.StageId);

            if (stagiaire == null || encadreur == null || stage == null)
            {
                return BadRequest("Stagiaire, encadreur ou stage introuvable");
            }

            // Vérifier si une fiche de pointage existe déjà pour ce stage
            var existingFiche = await _context.FichesDePointage
                .FirstOrDefaultAsync(f => f.StageId == fichePointageDto.StageId);

            if (existingFiche != null)
            {
                return Conflict("Une fiche de pointage existe déjà pour ce stage");
            }

            // Vérifier la durée du stage selon sa nature
            TimeSpan dureeStage = fichePointageDto.DateFinStage - fichePointageDto.DateDebutStage;
            if (fichePointageDto.NatureStage == NatureStage.StageImpregnation && dureeStage.TotalDays > 31)
            {
                return BadRequest("La durée d'un stage d'imprégnation ne doit pas dépasser un (1) mois");
            }

            if (fichePointageDto.NatureStage == NatureStage.StageFinEtude && dureeStage.TotalDays > 183)
            {
                return BadRequest("La durée d'un stage de mémoire ne doit pas dépasser six (6) mois");
            }

            var fichePointage = new FicheDePointage
            {
                DateCreation = DateTime.Now,
                NomPrenomStagiaire = fichePointageDto.NomPrenomStagiaire,
                StructureAccueil = fichePointageDto.StructureAccueil,
                NomQualitePersonneChargeSuivi = fichePointageDto.NomQualitePersonneChargeSuivi,
                DateDebutStage = fichePointageDto.DateDebutStage,
                DateFinStage = fichePointageDto.DateFinStage,
                NatureStage = fichePointageDto.NatureStage,
                EstValide = false,
                StagiaireId = fichePointageDto.StagiaireId,
                EncadreurId = fichePointageDto.EncadreurId,
                StageId = fichePointageDto.StageId
            };

            _context.FichesDePointage.Add(fichePointage);
            await _context.SaveChangesAsync();

            // Créer les mois de pointage pour la période du stage
            DateTime dateDebut = fichePointageDto.DateDebutStage;
            DateTime dateFin = fichePointageDto.DateFinStage;

            for (DateTime date = new DateTime(dateDebut.Year, dateDebut.Month, 1);
                 date <= dateFin;
                 date = date.AddMonths(1))
            {
                var pointageMois = new PointageMois
                {
                    Mois = date.ToString("MMMM"),
                    Annee = date.Year,
                    FicheDePointageId = fichePointage.Id
                };

                _context.PointageMois.Add(pointageMois);
                await _context.SaveChangesAsync();

                // Créer les jours de présence pour ce mois
                int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    DateTime jourDate = new DateTime(date.Year, date.Month, day);

                    // Ne créer que les jours qui sont dans la période du stage
                    if (jourDate >= dateDebut && jourDate <= dateFin)
                    {
                        var jourPresence = new JourPresence
                        {
                            Jour = day,
                            JourSemaine = jourDate.DayOfWeek,
                            EstPresent = false, // Par défaut, le stagiaire est marqué absent
                            Commentaire = "",
                            PointageMoisId = pointageMois.Id
                        };

                        _context.jourPresences.Add(jourPresence);
                    }
                }
                await _context.SaveChangesAsync();
            }

            // Recharger l'entité avec ses relations pour le DTO de retour
            fichePointage = await _context.FichesDePointage
                .Include(f => f.Stagiaire)
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .FirstOrDefaultAsync(f => f.Id == fichePointage.Id);

            return CreatedAtAction(nameof(GetFichePointage), new { id = fichePointage.Id },
                new FichePointageReadDto
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
                });
        }

        // PUT: api/FichePointage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFichePointage(int id, FichePointageUpdateDto fichePointageDto)
        {
            if (id != fichePointageDto.Id)
            {
                return BadRequest();
            }

            var fichePointage = await _context.FichesDePointage
                .Include(f => f.PointageMois)
                    .ThenInclude(p => p.JoursPresence)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fichePointage == null)
            {
                return NotFound();
            }

            // Vérifier la durée du stage selon sa nature
            TimeSpan dureeStage = fichePointageDto.DateFinStage - fichePointageDto.DateDebutStage;
            if (fichePointageDto.NatureStage == NatureStage.StageImpregnation && dureeStage.TotalDays > 31)
            {
                return BadRequest("La durée d'un stage d'imprégnation ne doit pas dépasser un (1) mois");
            }

            if (fichePointageDto.NatureStage == NatureStage.StageFinEtude && dureeStage.TotalDays > 183)
            {
                return BadRequest("La durée d'un stage de mémoire ne doit pas dépasser six (6) mois");
            }

            // Mettre à jour les propriétés de base
            fichePointage.NomPrenomStagiaire = fichePointageDto.NomPrenomStagiaire;
            fichePointage.StructureAccueil = fichePointageDto.StructureAccueil;
            fichePointage.NomQualitePersonneChargeSuivi = fichePointageDto.NomQualitePersonneChargeSuivi;
            fichePointage.DateDebutStage = fichePointageDto.DateDebutStage;
            fichePointage.DateFinStage = fichePointageDto.DateFinStage;
            fichePointage.NatureStage = fichePointageDto.NatureStage;

            // Mettre à jour les données de pointage si fournies
            if (!string.IsNullOrEmpty(fichePointageDto.DonneesPointage))
            {
                fichePointage.DonneesPointage = fichePointageDto.DonneesPointage;
            }

            // Mettre à jour les mois de pointage si fournis
            if (fichePointageDto.PointageMois != null && fichePointageDto.PointageMois.Any())
            {
                foreach (var moisDto in fichePointageDto.PointageMois)
                {
                    var moisExistant = fichePointage.PointageMois
                        .FirstOrDefault(m => m.Id == moisDto.Id);

                    if (moisExistant != null)
                    {
                        // Mettre à jour le mois existant
                        moisExistant.Mois = moisDto.Mois;
                        moisExistant.Annee = moisDto.Annee;

                        // Mettre à jour les jours de présence
                        if (moisDto.JoursPresence != null)
                        {
                            foreach (var jourDto in moisDto.JoursPresence)
                            {
                                var jourExistant = moisExistant.JoursPresence
                                    .FirstOrDefault(j => j.Id == jourDto.Id);

                                if (jourExistant != null)
                                {
                                    // Mettre à jour le jour existant
                                    jourExistant.EstPresent = jourDto.EstPresent;
                                    jourExistant.Commentaire = jourDto.Commentaire;
                                }
                            }
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

        // GET: api/FichePointage/{id}/PointageMois
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

        // PUT: api/FichePointage/{id}/PointageMois/{moisId}
        [HttpPut("{id}/PointageMois/{moisId}")]
        public async Task<IActionResult> UpdatePointageMois(int id, int moisId, PointageMoisUpdateDto pointageMoisDto)
        {
            var pointageMois = await _context.PointageMois
                .Include(p => p.JoursPresence)
                .FirstOrDefaultAsync(p => p.Id == moisId && p.FicheDePointageId == id);

            if (pointageMois == null)
            {
                return NotFound();
            }

            // Mettre à jour les jours de présence
            foreach (var jourDto in pointageMoisDto.JoursPresence)
            {
                var jour = pointageMois.JoursPresence.FirstOrDefault(j => j.Id == jourDto.Id);
                if (jour != null)
                {
                    jour.EstPresent = jourDto.EstPresent;
                    jour.Commentaire = jourDto.Commentaire;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/FichePointage/UpdatePointage
        [HttpPatch("UpdatePointage")]
        public async Task<IActionResult> UpdateDonneesPointage(DonneesPointageUpdateDto donneesPointageDto)
        {
            var fichePointage = await _context.FichesDePointage.FindAsync(donneesPointageDto.FichePointageId);
            if (fichePointage == null)
            {
                return NotFound();
            }

            // Valider le format JSON des données de pointage
            try
            {
                var pointageData = JsonSerializer.Deserialize<object>(donneesPointageDto.DonneesPointage);
            }
            catch (JsonException)
            {
                return BadRequest("Format JSON invalide pour les données de pointage");
            }

            fichePointage.DonneesPointage = donneesPointageDto.DonneesPointage;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichePointageExists(donneesPointageDto.FichePointageId))
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

        // POST: api/FichePointage/Validate
        [HttpPost("Validate")]
        public async Task<IActionResult> ValidateFichePointage(FichePointageValidationDto validationDto)
        {
            var fichePointage = await _context.FichesDePointage.FindAsync(validationDto.Id);
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
                if (!FichePointageExists(validationDto.Id))
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

        // DELETE: api/FichePointage/5
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

        // GET: api/FichePointage/Stagiaire/5
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

        // GET: api/FichePointage/Encadreur/5
        [HttpGet("Encadreur/{encadreurId}")]
        [Authorize(Roles = "Direction,Encadreur")]
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

    public class PointageMoisUpdateDto
    {
        public List<JourPresenceUpdateDto> JoursPresence { get; set; }
    }

    public class JourPresenceUpdateDto
    {
        public int Id { get; set; }
        public bool EstPresent { get; set; }
        public string Commentaire { get; set; }
    }

    public class DonneesPointageUpdateDto
    {
        [Required(ErrorMessage = "L'identifiant de la fiche de pointage est obligatoire")]
        public int FichePointageId { get; set; }

        [Required(ErrorMessage = "Les données de pointage sont obligatoires")]
        public string DonneesPointage { get; set; }
    }
}
