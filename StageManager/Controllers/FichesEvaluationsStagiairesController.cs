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

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FicheEvaluationStagiaireController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FicheEvaluationStagiaireController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FicheEvaluationStagiaire
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FicheEvaluationStagiaireListDto>>> GetFichesEvaluationStagiaire()
        {
            var fichesEvaluation = await _context.FichesEvaluationStagiaire
                .Include(f => f.Stagiaire)
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .ToListAsync();

            return fichesEvaluation.Select(f => new FicheEvaluationStagiaireListDto
            {
                Id = f.Id,
                NomPrenomStagiaire = f.NomPrenomStagiaire,
                FormationStagiaire = f.FormationStagiaire,
                ThemeStage = f.ThemeStage,
                NomPrenomEncadreur = f.NomPrenomEncadreur,
                DateEvaluation = f.DateEvaluation,
                ScoreMoyen = CalculerScoreMoyen(f)
            }).ToList();
        }

        // GET: api/FicheEvaluationStagiaire/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FicheEvaluationStagiaireReadDto>> GetFicheEvaluationStagiaire(int id)
        {
            var ficheEvaluation = await _context.FichesEvaluationStagiaire
                .Include(f => f.Stagiaire)
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            return new FicheEvaluationStagiaireReadDto
            {
                Id = ficheEvaluation.Id,
                DateCreation = ficheEvaluation.DateCreation,
                NomPrenomStagiaire = ficheEvaluation.NomPrenomStagiaire,
                FormationStagiaire = ficheEvaluation.FormationStagiaire,
                DureeStage = ficheEvaluation.DureeStage,
                PeriodeDu = ficheEvaluation.PeriodeDu,
                PeriodeAu = ficheEvaluation.PeriodeAu,
                StructureAccueil = ficheEvaluation.StructureAccueil,
                NombreSeancesPrevues = ficheEvaluation.NombreSeancesPrevues,
                NomPrenomEncadreur = ficheEvaluation.NomPrenomEncadreur,
                FonctionEncadreur = ficheEvaluation.FonctionEncadreur,
                ThemeStage = ficheEvaluation.ThemeStage,
                MissionsConfieesAuStagiaire = ficheEvaluation.MissionsConfieesAuStagiaire,

                // Critères d'évaluation
                RealisationMissionsConfiees = ficheEvaluation.RealisationMissionsConfiees,
                RespectDelaisProcedures = ficheEvaluation.RespectDelaisProcedures,
                ComprehensionTravaux = ficheEvaluation.ComprehensionTravaux,
                AppreciationRenduTravail = ficheEvaluation.AppreciationRenduTravail,
                UtilisationMoyensMisDisposition = ficheEvaluation.UtilisationMoyensMisDisposition,
                NiveauConnaissances = ficheEvaluation.NiveauConnaissances,
                CompetencesGenerales = ficheEvaluation.CompetencesGenerales,
                AdaptationOrganisationMethodesTravail = ficheEvaluation.AdaptationOrganisationMethodesTravail,
                PonctualiteAssiduite = ficheEvaluation.PonctualiteAssiduite,
                RigueurSerieux = ficheEvaluation.RigueurSerieux,
                DisponibiliteMotivationEngagement = ficheEvaluation.DisponibiliteMotivationEngagement,
                IntegrationSeinService = ficheEvaluation.IntegrationSeinService,
                Aptitudes = ficheEvaluation.Aptitudes,
                TravailEquipe = ficheEvaluation.TravailEquipe,
                CapaciteApprendreComprendre = ficheEvaluation.CapaciteApprendreComprendre,
                ApplicationConnaissancesNouvelles = ficheEvaluation.ApplicationConnaissancesNouvelles,

                // Sections spécifiques
                RechercheInformations = ficheEvaluation.RechercheInformations,
                MethodeOrganisationTravail = ficheEvaluation.MethodeOrganisationTravail,
                AnalyseExplicationSynthese = ficheEvaluation.AnalyseExplicationSynthese,
                Communication = ficheEvaluation.Communication,

                AppreciationGlobaleTuteur = ficheEvaluation.AppreciationGlobaleTuteur,
                Observations = ficheEvaluation.Observations,
                NomPrenomEvaluateur = ficheEvaluation.NomPrenomEvaluateur,
                DateEvaluation = ficheEvaluation.DateEvaluation,

                Stagiaire = new StagiaireMinimalDto
                {
                    Id = ficheEvaluation.Stagiaire.Id,
                    NomPrenom = $"{ficheEvaluation.Stagiaire.Nom} {ficheEvaluation.Stagiaire.Prenom}",
                    Email = ficheEvaluation.Stagiaire.Email
                },

                Encadreur = new EncadreurMinimalDto
                {
                    Id = ficheEvaluation.Encadreur.Id,
                    NomPrenom = $"{ficheEvaluation.Encadreur.Nom} {ficheEvaluation.Encadreur.Prenom}",
                    Fonction = ficheEvaluation.Encadreur.Fonction,
                    Departement = ficheEvaluation.Encadreur.Departement
                },

                Stage = new StageMinimalDto
                {
                    Id = ficheEvaluation.Stage.Id,
                    DateDebut = ficheEvaluation.Stage.DateDebut,
                    DateFin = ficheEvaluation.Stage.DateFin
                }
            };
        }
        // PUT: api/FicheEvaluationStagiaire/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFicheEvaluationStagiaire(int id, FicheEvaluationStagiaireUpdateDto ficheEvaluationDto)
        {


            var ficheEvaluation = await _context.FichesEvaluationStagiaire.FindAsync(id);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            // Vérifier que la fiche n'a pas été créée depuis plus de 7 jours
            if ((DateTime.Now - ficheEvaluation.DateCreation).TotalDays > 7)
            {
                return BadRequest("L'évaluation ne peut plus être modifiée après 7 jours");
            }

            ficheEvaluation.MissionsConfieesAuStagiaire = ficheEvaluationDto.MissionsConfieesAuStagiaire;

            // Critères d'évaluation
            ficheEvaluation.RealisationMissionsConfiees = ficheEvaluationDto.RealisationMissionsConfiees;
            ficheEvaluation.RespectDelaisProcedures = ficheEvaluationDto.RespectDelaisProcedures;
            ficheEvaluation.ComprehensionTravaux = ficheEvaluationDto.ComprehensionTravaux;
            ficheEvaluation.AppreciationRenduTravail = ficheEvaluationDto.AppreciationRenduTravail;
            ficheEvaluation.UtilisationMoyensMisDisposition = ficheEvaluationDto.UtilisationMoyensMisDisposition;
            ficheEvaluation.NiveauConnaissances = ficheEvaluationDto.NiveauConnaissances;
            ficheEvaluation.CompetencesGenerales = ficheEvaluationDto.CompetencesGenerales;
            ficheEvaluation.AdaptationOrganisationMethodesTravail = ficheEvaluationDto.AdaptationOrganisationMethodesTravail;
            ficheEvaluation.PonctualiteAssiduite = ficheEvaluationDto.PonctualiteAssiduite;
            ficheEvaluation.RigueurSerieux = ficheEvaluationDto.RigueurSerieux;
            ficheEvaluation.DisponibiliteMotivationEngagement = ficheEvaluationDto.DisponibiliteMotivationEngagement;
            ficheEvaluation.IntegrationSeinService = ficheEvaluationDto.IntegrationSeinService;
            ficheEvaluation.Aptitudes = ficheEvaluationDto.Aptitudes;
            ficheEvaluation.TravailEquipe = ficheEvaluationDto.TravailEquipe;
            ficheEvaluation.CapaciteApprendreComprendre = ficheEvaluationDto.CapaciteApprendreComprendre;
            ficheEvaluation.ApplicationConnaissancesNouvelles = ficheEvaluationDto.ApplicationConnaissancesNouvelles;

            // Sections spécifiques
            ficheEvaluation.RechercheInformations = ficheEvaluationDto.RechercheInformations;
            ficheEvaluation.MethodeOrganisationTravail = ficheEvaluationDto.MethodeOrganisationTravail;
            ficheEvaluation.AnalyseExplicationSynthese = ficheEvaluationDto.AnalyseExplicationSynthese;
            ficheEvaluation.Communication = ficheEvaluationDto.Communication;

            ficheEvaluation.AppreciationGlobaleTuteur = ficheEvaluationDto.AppreciationGlobaleTuteur;
            ficheEvaluation.Observations = ficheEvaluationDto.Observations;
            ficheEvaluation.NomPrenomEvaluateur = ficheEvaluationDto.NomPrenomEvaluateur;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FicheEvaluationStagiaireExists(id))
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

        // DELETE: api/FicheEvaluationStagiaire/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFicheEvaluationStagiaire(int id)
        {
            var ficheEvaluation = await _context.FichesEvaluationStagiaire.FindAsync(id);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            // Vérifier que la fiche n'a pas été créée depuis plus de 7 jours (pour les encadreurs)
            if (User.IsInRole("Encadreur") && (DateTime.Now - ficheEvaluation.DateCreation).TotalDays > 7)
            {
                return BadRequest("L'évaluation ne peut plus être supprimée après 7 jours");
            }

            _context.FichesEvaluationStagiaire.Remove(ficheEvaluation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/FicheEvaluationStagiaire/Stagiaire/5
        [HttpGet("Stagiaire/{stagiaireId}")]
        public async Task<ActionResult<FicheEvaluationStagiaireReadDto>> GetFicheEvaluationByStagiaire(int stagiaireId)
        {
            var ficheEvaluation = await _context.FichesEvaluationStagiaire
                .Include(f => f.Stagiaire)
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .FirstOrDefaultAsync(f => f.StagiaireId == stagiaireId);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            // Vérifier que l'utilisateur a le droit d'accéder à cette évaluation
            if (User.IsInRole("Stagiaire") && !User.HasClaim(c => c.Type == "UserId" && c.Value == stagiaireId.ToString()))
            {
                return Forbid();
            }

            return new FicheEvaluationStagiaireReadDto
            {
                Id = ficheEvaluation.Id,
                DateCreation = ficheEvaluation.DateCreation,
                NomPrenomStagiaire = ficheEvaluation.NomPrenomStagiaire,
                FormationStagiaire = ficheEvaluation.FormationStagiaire,
                DureeStage = ficheEvaluation.DureeStage,
                PeriodeDu = ficheEvaluation.PeriodeDu,
                PeriodeAu = ficheEvaluation.PeriodeAu,
                StructureAccueil = ficheEvaluation.StructureAccueil,
                NombreSeancesPrevues = ficheEvaluation.NombreSeancesPrevues,
                NomPrenomEncadreur = ficheEvaluation.NomPrenomEncadreur,
                FonctionEncadreur = ficheEvaluation.FonctionEncadreur,
                ThemeStage = ficheEvaluation.ThemeStage,
                MissionsConfieesAuStagiaire = ficheEvaluation.MissionsConfieesAuStagiaire,

                // Toutes les propriétés d'évaluation...
                RealisationMissionsConfiees = ficheEvaluation.RealisationMissionsConfiees,
                RespectDelaisProcedures = ficheEvaluation.RespectDelaisProcedures,
                ComprehensionTravaux = ficheEvaluation.ComprehensionTravaux,
                AppreciationRenduTravail = ficheEvaluation.AppreciationRenduTravail,
                UtilisationMoyensMisDisposition = ficheEvaluation.UtilisationMoyensMisDisposition,
                NiveauConnaissances = ficheEvaluation.NiveauConnaissances,
                CompetencesGenerales = ficheEvaluation.CompetencesGenerales,
                AdaptationOrganisationMethodesTravail = ficheEvaluation.AdaptationOrganisationMethodesTravail,
                PonctualiteAssiduite = ficheEvaluation.PonctualiteAssiduite,
                RigueurSerieux = ficheEvaluation.RigueurSerieux,
                DisponibiliteMotivationEngagement = ficheEvaluation.DisponibiliteMotivationEngagement,
                IntegrationSeinService = ficheEvaluation.IntegrationSeinService,
                Aptitudes = ficheEvaluation.Aptitudes,
                TravailEquipe = ficheEvaluation.TravailEquipe,
                CapaciteApprendreComprendre = ficheEvaluation.CapaciteApprendreComprendre,
                ApplicationConnaissancesNouvelles = ficheEvaluation.ApplicationConnaissancesNouvelles,
                RechercheInformations = ficheEvaluation.RechercheInformations,
                MethodeOrganisationTravail = ficheEvaluation.MethodeOrganisationTravail,
                AnalyseExplicationSynthese = ficheEvaluation.AnalyseExplicationSynthese,
                Communication = ficheEvaluation.Communication,

                AppreciationGlobaleTuteur = ficheEvaluation.AppreciationGlobaleTuteur,
                Observations = ficheEvaluation.Observations,
                NomPrenomEvaluateur = ficheEvaluation.NomPrenomEvaluateur,
                DateEvaluation = ficheEvaluation.DateEvaluation,

                Stagiaire = new StagiaireMinimalDto
                {
                    Id = ficheEvaluation.Stagiaire.Id,
                    NomPrenom = $"{ficheEvaluation.Stagiaire.Nom} {ficheEvaluation.Stagiaire.Prenom}",
                    Email = ficheEvaluation.Stagiaire.Email
                },

                Encadreur = new EncadreurMinimalDto
                {
                    Id = ficheEvaluation.Encadreur.Id,
                    NomPrenom = $"{ficheEvaluation.Encadreur.Nom} {ficheEvaluation.Encadreur.Prenom}",
                    Fonction = ficheEvaluation.Encadreur.Fonction,
                    Departement = ficheEvaluation.Encadreur.Departement
                },

                Stage = new StageMinimalDto
                {
                    Id = ficheEvaluation.Stage.Id,
                    DateDebut = ficheEvaluation.Stage.DateDebut,
                    DateFin = ficheEvaluation.Stage.DateFin
                }
            };
        }

        // GET: api/FicheEvaluationStagiaire/Encadreur/5
        [HttpGet("Encadreur/{encadreurId}")]
        public async Task<ActionResult<IEnumerable<FicheEvaluationStagiaireListDto>>> GetFichesEvaluationByEncadreur(int encadreurId)
        {
            var fichesEvaluation = await _context.FichesEvaluationStagiaire
                .Where(f => f.EncadreurId == encadreurId)
                .ToListAsync();

            return fichesEvaluation.Select(f => new FicheEvaluationStagiaireListDto
            {
                Id = f.Id,
                NomPrenomStagiaire = f.NomPrenomStagiaire,
                FormationStagiaire = f.FormationStagiaire,
                ThemeStage = f.ThemeStage,
                NomPrenomEncadreur = f.NomPrenomEncadreur,
                DateEvaluation = f.DateEvaluation,
                ScoreMoyen = CalculerScoreMoyen(f)
            }).ToList();
        }

        // GET: api/FicheEvaluationStagiaire/Stage/5
        [HttpGet("Stage/{stageId}")]
        public async Task<ActionResult<FicheEvaluationStagiaireReadDto>> GetFicheEvaluationByStage(int stageId)
        {
            var ficheEvaluation = await _context.FichesEvaluationStagiaire
                .Include(f => f.Stagiaire)
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .FirstOrDefaultAsync(f => f.StageId == stageId);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            // Vérifier que l'utilisateur a le droit d'accéder à cette évaluation
            if (User.IsInRole("Stagiaire") && !User.HasClaim(c => c.Type == "UserId" && c.Value == ficheEvaluation.StagiaireId.ToString()))
            {
                return Forbid();
            }

            if (User.IsInRole("Encadreur") && !User.HasClaim(c => c.Type == "UserId" && c.Value == ficheEvaluation.EncadreurId.ToString()))
            {
                return Forbid();
            }

            return new FicheEvaluationStagiaireReadDto
            {
                Id = ficheEvaluation.Id,
                DateCreation = ficheEvaluation.DateCreation,
                NomPrenomStagiaire = ficheEvaluation.NomPrenomStagiaire,
                FormationStagiaire = ficheEvaluation.FormationStagiaire,
                DureeStage = ficheEvaluation.DureeStage,
                PeriodeDu = ficheEvaluation.PeriodeDu,
                PeriodeAu = ficheEvaluation.PeriodeAu,
                StructureAccueil = ficheEvaluation.StructureAccueil,
                NombreSeancesPrevues = ficheEvaluation.NombreSeancesPrevues,
                NomPrenomEncadreur = ficheEvaluation.NomPrenomEncadreur,
                FonctionEncadreur = ficheEvaluation.FonctionEncadreur,
                ThemeStage = ficheEvaluation.ThemeStage,
                MissionsConfieesAuStagiaire = ficheEvaluation.MissionsConfieesAuStagiaire,

                // Toutes les propriétés d'évaluation...
                RealisationMissionsConfiees = ficheEvaluation.RealisationMissionsConfiees,
                RespectDelaisProcedures = ficheEvaluation.RespectDelaisProcedures,
                ComprehensionTravaux = ficheEvaluation.ComprehensionTravaux,
                AppreciationRenduTravail = ficheEvaluation.AppreciationRenduTravail,
                UtilisationMoyensMisDisposition = ficheEvaluation.UtilisationMoyensMisDisposition,
                NiveauConnaissances = ficheEvaluation.NiveauConnaissances,
                CompetencesGenerales = ficheEvaluation.CompetencesGenerales,
                AdaptationOrganisationMethodesTravail = ficheEvaluation.AdaptationOrganisationMethodesTravail,
                PonctualiteAssiduite = ficheEvaluation.PonctualiteAssiduite,
                RigueurSerieux = ficheEvaluation.RigueurSerieux,
                DisponibiliteMotivationEngagement = ficheEvaluation.DisponibiliteMotivationEngagement,
                IntegrationSeinService = ficheEvaluation.IntegrationSeinService,
                Aptitudes = ficheEvaluation.Aptitudes,
                TravailEquipe = ficheEvaluation.TravailEquipe,
                CapaciteApprendreComprendre = ficheEvaluation.CapaciteApprendreComprendre,
                ApplicationConnaissancesNouvelles = ficheEvaluation.ApplicationConnaissancesNouvelles,
                RechercheInformations = ficheEvaluation.RechercheInformations,
                MethodeOrganisationTravail = ficheEvaluation.MethodeOrganisationTravail,
                AnalyseExplicationSynthese = ficheEvaluation.AnalyseExplicationSynthese,
                Communication = ficheEvaluation.Communication,

                AppreciationGlobaleTuteur = ficheEvaluation.AppreciationGlobaleTuteur,
                Observations = ficheEvaluation.Observations,
                NomPrenomEvaluateur = ficheEvaluation.NomPrenomEvaluateur,
                DateEvaluation = ficheEvaluation.DateEvaluation,

                Stagiaire = new StagiaireMinimalDto
                {
                    Id = ficheEvaluation.Stagiaire.Id,
                    NomPrenom = $"{ficheEvaluation.Stagiaire.Nom} {ficheEvaluation.Stagiaire.Prenom}",
                    Email = ficheEvaluation.Stagiaire.Email
                },

                Encadreur = new EncadreurMinimalDto
                {
                    Id = ficheEvaluation.Encadreur.Id,
                    NomPrenom = $"{ficheEvaluation.Encadreur.Nom} {ficheEvaluation.Encadreur.Prenom}",
                    Fonction = ficheEvaluation.Encadreur.Fonction,
                    Departement = ficheEvaluation.Encadreur.Departement
                },

                Stage = new StageMinimalDto
                {
                    Id = ficheEvaluation.Stage.Id,
                    DateDebut = ficheEvaluation.Stage.DateDebut,
                    DateFin = ficheEvaluation.Stage.DateFin
                }
            };
        }

        // Méthode utilitaire pour calculer le score moyen
        private double CalculerScoreMoyen(FicheEvaluationStagiaire ficheEvaluation)
        {
            int totalCriteres = 20; // Nombre total de critères d'évaluation

            double somme = (int)ficheEvaluation.RealisationMissionsConfiees +
                          (int)ficheEvaluation.RespectDelaisProcedures +
                          (int)ficheEvaluation.ComprehensionTravaux +
                          (int)ficheEvaluation.AppreciationRenduTravail +
                          (int)ficheEvaluation.UtilisationMoyensMisDisposition +
                          (int)ficheEvaluation.NiveauConnaissances +
                          (int)ficheEvaluation.CompetencesGenerales +
                          (int)ficheEvaluation.AdaptationOrganisationMethodesTravail +
                          (int)ficheEvaluation.PonctualiteAssiduite +
                          (int)ficheEvaluation.RigueurSerieux +
                          (int)ficheEvaluation.DisponibiliteMotivationEngagement +
                          (int)ficheEvaluation.IntegrationSeinService +
                          (int)ficheEvaluation.Aptitudes +
                          (int)ficheEvaluation.TravailEquipe +
                          (int)ficheEvaluation.CapaciteApprendreComprendre +
                          (int)ficheEvaluation.ApplicationConnaissancesNouvelles +
                          (int)ficheEvaluation.RechercheInformations +
                          (int)ficheEvaluation.MethodeOrganisationTravail +
                          (int)ficheEvaluation.AnalyseExplicationSynthese +
                          (int)ficheEvaluation.Communication;

            return Math.Round(somme / totalCriteres, 2);
        }

        private bool FicheEvaluationStagiaireExists(int id)
        {
            return _context.FichesEvaluationStagiaire.Any(e => e.Id == id);
        }
    }
}