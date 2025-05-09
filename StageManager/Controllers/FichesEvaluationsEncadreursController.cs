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
    public class FicheEvaluationEncadreurController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FicheEvaluationEncadreurController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FicheEvaluationEncadreur
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FicheEvaluationEncadreurListDto>>> GetFichesEvaluationEncadreur()
        {
            var fichesEvaluation = await _context.FichesEvaluationEncadreur
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .ToListAsync();

            return fichesEvaluation.Select(f => new FicheEvaluationEncadreurListDto
            {
                Id = f.Id,
                NomPrenomEncadreur = f.NomPrenomEncadreur,
                FonctionEncadreur = f.FonctionEncadreur,
                DateEvaluation = f.DateEvaluation,
                NomPrenomStagiaireEvaluateur = f.NomPrenomStagiaireEvaluateur,
                StageId = f.StageId,
                ScoreMoyen = CalculerScoreMoyen(f)
            }).ToList();
        }

        // GET: api/FicheEvaluationEncadreur/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FicheEvaluationEncadreurReadDto>> GetFicheEvaluationEncadreur(int id)
        {
            var ficheEvaluation = await _context.FichesEvaluationEncadreur
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            return new FicheEvaluationEncadreurReadDto
            {
                Id = ficheEvaluation.Id,
                DateCreation = ficheEvaluation.DateCreation,
                NomPrenomEncadreur = ficheEvaluation.NomPrenomEncadreur,
                FonctionEncadreur = ficheEvaluation.FonctionEncadreur,
                DateDebutStage = ficheEvaluation.DateDebutStage,
                DateFinStage = ficheEvaluation.DateFinStage,
                StageId = ficheEvaluation.StageId,


                // Catégorie 1: Planification du travail
                FixeObjectifsClairs = ficheEvaluation.FixeObjectifsClairs,
                GereImprevus = ficheEvaluation.GereImprevus,
                RencontreRegulierementEtudiants = ficheEvaluation.RencontreRegulierementEtudiants,
                OrganiseEtapesRecherche = ficheEvaluation.OrganiseEtapesRecherche,

                // Catégorie 2: Comprendre et faire comprendre
                ExpliqueClairementContenu = ficheEvaluation.ExpliqueClairementContenu,
                InterrogeEtudiantsFeedback = ficheEvaluation.InterrogeEtudiantsFeedback,
                MaitriseConnaissances = ficheEvaluation.MaitriseConnaissances,
                EnseigneFaitDemonstrations = ficheEvaluation.EnseigneFaitDemonstrations,

                // Catégorie 3: Susciter la participation
                InviteEtudiantsQuestions = ficheEvaluation.InviteEtudiantsQuestions,
                RepondQuestionsEtudiants = ficheEvaluation.RepondQuestionsEtudiants,
                EncourageInitiativesEtudiants = ficheEvaluation.EncourageInitiativesEtudiants,
                InterrogeEtudiantsTravailEffectue = ficheEvaluation.InterrogeEtudiantsTravailEffectue,
                AccepteExpressionPointsVueDifferents = ficheEvaluation.AccepteExpressionPointsVueDifferents,

                // Catégorie 4: Communication orale
                CommuniqueClairementSimplement = ficheEvaluation.CommuniqueClairementSimplement,
                CritiqueConstructive = ficheEvaluation.CritiqueConstructive,
                PondereQuantiteInformation = ficheEvaluation.PondereQuantiteInformation,

                // Catégorie 5: Sens de responsabilité
                EfficaceGestionSupervision = ficheEvaluation.EfficaceGestionSupervision,
                MaintientAttitudeProfessionnelle = ficheEvaluation.MaintientAttitudeProfessionnelle,
                TransmetDonneesFiables = ficheEvaluation.TransmetDonneesFiables,

                // Catégorie 6: Stimuler la motivation des étudiants
                OrienteEtudiantsRessourcesPertinentes = ficheEvaluation.OrienteEtudiantsRessourcesPertinentes,
                MontreImportanceSujetTraite = ficheEvaluation.MontreImportanceSujetTraite,
                ProdigueEncouragementsFeedback = ficheEvaluation.ProdigueEncouragementsFeedback,
                DemontreInteretRecherche = ficheEvaluation.DemontreInteretRecherche,

                Observations = ficheEvaluation.Observations,
                NomPrenomStagiaireEvaluateur = ficheEvaluation.NomPrenomStagiaireEvaluateur,
                DateEvaluation = ficheEvaluation.DateEvaluation,

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

        // PUT: api/FicheEvaluationEncadreur/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFicheEvaluationEncadreur(int id, FicheEvaluationEncadreurUpdateDto ficheEvaluationDto)
        {

            var ficheEvaluation = await _context.FichesEvaluationEncadreur.FindAsync(id);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            // Vérifier que la fiche n'a pas été créée depuis plus de 48 heures
            //if ((DateTime.Now - ficheEvaluation.DateCreation).TotalHours > 48)
            // {
            //    return BadRequest("L'évaluation ne peut plus être modifiée après 48 heures");
            //}
            // Catégorie 1: Planification du travail
            ficheEvaluation.FixeObjectifsClairs = ficheEvaluationDto.FixeObjectifsClairs;
            ficheEvaluation.GereImprevus = ficheEvaluationDto.GereImprevus;
            ficheEvaluation.RencontreRegulierementEtudiants = ficheEvaluationDto.RencontreRegulierementEtudiants;
            ficheEvaluation.OrganiseEtapesRecherche = ficheEvaluationDto.OrganiseEtapesRecherche;

            // Catégorie 2: Comprendre et faire comprendre
            ficheEvaluation.ExpliqueClairementContenu = ficheEvaluationDto.ExpliqueClairementContenu;
            ficheEvaluation.InterrogeEtudiantsFeedback = ficheEvaluationDto.InterrogeEtudiantsFeedback;
            ficheEvaluation.MaitriseConnaissances = ficheEvaluationDto.MaitriseConnaissances;
            ficheEvaluation.EnseigneFaitDemonstrations = ficheEvaluationDto.EnseigneFaitDemonstrations;

            // Catégorie 3: Susciter la participation
            ficheEvaluation.InviteEtudiantsQuestions = ficheEvaluationDto.InviteEtudiantsQuestions;
            ficheEvaluation.RepondQuestionsEtudiants = ficheEvaluationDto.RepondQuestionsEtudiants;
            ficheEvaluation.EncourageInitiativesEtudiants = ficheEvaluationDto.EncourageInitiativesEtudiants;
            ficheEvaluation.InterrogeEtudiantsTravailEffectue = ficheEvaluationDto.InterrogeEtudiantsTravailEffectue;
            ficheEvaluation.AccepteExpressionPointsVueDifferents = ficheEvaluationDto.AccepteExpressionPointsVueDifferents;

            // Catégorie 4: Communication orale
            ficheEvaluation.CommuniqueClairementSimplement = ficheEvaluationDto.CommuniqueClairementSimplement;
            ficheEvaluation.CritiqueConstructive = ficheEvaluationDto.CritiqueConstructive;
            ficheEvaluation.PondereQuantiteInformation = ficheEvaluationDto.PondereQuantiteInformation;

            // Catégorie 5: Sens de responsabilité
            ficheEvaluation.EfficaceGestionSupervision = ficheEvaluationDto.EfficaceGestionSupervision;
            ficheEvaluation.MaintientAttitudeProfessionnelle = ficheEvaluationDto.MaintientAttitudeProfessionnelle;
            ficheEvaluation.TransmetDonneesFiables = ficheEvaluationDto.TransmetDonneesFiables;

            // Catégorie 6: Stimuler la motivation des étudiants
            ficheEvaluation.OrienteEtudiantsRessourcesPertinentes = ficheEvaluationDto.OrienteEtudiantsRessourcesPertinentes;
            ficheEvaluation.MontreImportanceSujetTraite = ficheEvaluationDto.MontreImportanceSujetTraite;
            ficheEvaluation.ProdigueEncouragementsFeedback = ficheEvaluationDto.ProdigueEncouragementsFeedback;
            ficheEvaluation.DemontreInteretRecherche = ficheEvaluationDto.DemontreInteretRecherche;

            ficheEvaluation.Observations = ficheEvaluationDto.Observations;
            ficheEvaluation.NomPrenomStagiaireEvaluateur = ficheEvaluationDto.NomPrenomStagiaireEvaluateur;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FicheEvaluationEncadreurExists(id))
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

        // DELETE: api/FicheEvaluationEncadreur/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFicheEvaluationEncadreur(int id)
        {
            var ficheEvaluation = await _context.FichesEvaluationEncadreur.FindAsync(id);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            // Vérifier que la fiche n'a pas été créée depuis plus de 48 heures (pour les stagiaires)
            if (User.IsInRole("Stagiaire") && (DateTime.Now - ficheEvaluation.DateCreation).TotalHours > 48)
            {
                return BadRequest("L'évaluation ne peut plus être supprimée après 48 heures");
            }

            _context.FichesEvaluationEncadreur.Remove(ficheEvaluation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/FicheEvaluationEncadreur/Encadreur/5
        [HttpGet("Encadreur/{encadreurId}")]
        public async Task<ActionResult<IEnumerable<FicheEvaluationEncadreurListDto>>> GetFichesEvaluationByEncadreur(int encadreurId)
        {
            var fichesEvaluation = await _context.FichesEvaluationEncadreur
                .Where(f => f.EncadreurId == encadreurId)
                .ToListAsync();

            return fichesEvaluation.Select(f => new FicheEvaluationEncadreurListDto
            {
                Id = f.Id,
                NomPrenomEncadreur = f.NomPrenomEncadreur,
                FonctionEncadreur = f.FonctionEncadreur,
                DateEvaluation = f.DateEvaluation,
                NomPrenomStagiaireEvaluateur = f.NomPrenomStagiaireEvaluateur,
                StageId = f.StageId,
                ScoreMoyen = CalculerScoreMoyen(f)
            }).ToList();
        }

        // GET: api/FicheEvaluationEncadreur/Stage/5
        [HttpGet("Stage/{stageId}")]
        public async Task<ActionResult<FicheEvaluationEncadreurReadDto>> GetFicheEvaluationByStage(int stageId)
        {
            var ficheEvaluation = await _context.FichesEvaluationEncadreur
                .Include(f => f.Encadreur)
                .Include(f => f.Stage)
                .FirstOrDefaultAsync(f => f.StageId == stageId);

            if (ficheEvaluation == null)
            {
                return NotFound();
            }

            return new FicheEvaluationEncadreurReadDto
            {
                Id = ficheEvaluation.Id,
                DateCreation = ficheEvaluation.DateCreation,
                NomPrenomEncadreur = ficheEvaluation.NomPrenomEncadreur,
                FonctionEncadreur = ficheEvaluation.FonctionEncadreur,
                DateDebutStage = ficheEvaluation.DateDebutStage,
                DateFinStage = ficheEvaluation.DateFinStage,

                // Toutes les propriétés d'évaluation...
                FixeObjectifsClairs = ficheEvaluation.FixeObjectifsClairs,
                GereImprevus = ficheEvaluation.GereImprevus,
                RencontreRegulierementEtudiants = ficheEvaluation.RencontreRegulierementEtudiants,
                OrganiseEtapesRecherche = ficheEvaluation.OrganiseEtapesRecherche,
                ExpliqueClairementContenu = ficheEvaluation.ExpliqueClairementContenu,
                InterrogeEtudiantsFeedback = ficheEvaluation.InterrogeEtudiantsFeedback,
                MaitriseConnaissances = ficheEvaluation.MaitriseConnaissances,
                EnseigneFaitDemonstrations = ficheEvaluation.EnseigneFaitDemonstrations,
                InviteEtudiantsQuestions = ficheEvaluation.InviteEtudiantsQuestions,
                RepondQuestionsEtudiants = ficheEvaluation.RepondQuestionsEtudiants,
                EncourageInitiativesEtudiants = ficheEvaluation.EncourageInitiativesEtudiants,
                InterrogeEtudiantsTravailEffectue = ficheEvaluation.InterrogeEtudiantsTravailEffectue,
                AccepteExpressionPointsVueDifferents = ficheEvaluation.AccepteExpressionPointsVueDifferents,
                CommuniqueClairementSimplement = ficheEvaluation.CommuniqueClairementSimplement,
                CritiqueConstructive = ficheEvaluation.CritiqueConstructive,
                PondereQuantiteInformation = ficheEvaluation.PondereQuantiteInformation,
                EfficaceGestionSupervision = ficheEvaluation.EfficaceGestionSupervision,
                MaintientAttitudeProfessionnelle = ficheEvaluation.MaintientAttitudeProfessionnelle,
                TransmetDonneesFiables = ficheEvaluation.TransmetDonneesFiables,
                OrienteEtudiantsRessourcesPertinentes = ficheEvaluation.OrienteEtudiantsRessourcesPertinentes,
                MontreImportanceSujetTraite = ficheEvaluation.MontreImportanceSujetTraite,
                ProdigueEncouragementsFeedback = ficheEvaluation.ProdigueEncouragementsFeedback,
                DemontreInteretRecherche = ficheEvaluation.DemontreInteretRecherche,

                Observations = ficheEvaluation.Observations,
                NomPrenomStagiaireEvaluateur = ficheEvaluation.NomPrenomStagiaireEvaluateur,
                DateEvaluation = ficheEvaluation.DateEvaluation,

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
        private double CalculerScoreMoyen(FicheEvaluationEncadreur ficheEvaluation)
        {
            int totalCriteres = 23; // Nombre total de critères d'évaluation

            double somme = (int)ficheEvaluation.FixeObjectifsClairs +
                          (int)ficheEvaluation.GereImprevus +
                          (int)ficheEvaluation.RencontreRegulierementEtudiants +
                          (int)ficheEvaluation.OrganiseEtapesRecherche +
                          (int)ficheEvaluation.ExpliqueClairementContenu +
                          (int)ficheEvaluation.InterrogeEtudiantsFeedback +
                          (int)ficheEvaluation.MaitriseConnaissances +
                          (int)ficheEvaluation.EnseigneFaitDemonstrations +
                          (int)ficheEvaluation.InviteEtudiantsQuestions +
                          (int)ficheEvaluation.RepondQuestionsEtudiants +
                          (int)ficheEvaluation.EncourageInitiativesEtudiants +
                          (int)ficheEvaluation.InterrogeEtudiantsTravailEffectue +
                          (int)ficheEvaluation.AccepteExpressionPointsVueDifferents +
                          (int)ficheEvaluation.CommuniqueClairementSimplement +
                          (int)ficheEvaluation.CritiqueConstructive +
                          (int)ficheEvaluation.PondereQuantiteInformation +
                          (int)ficheEvaluation.EfficaceGestionSupervision +
                          (int)ficheEvaluation.MaintientAttitudeProfessionnelle +
                          (int)ficheEvaluation.TransmetDonneesFiables +
                          (int)ficheEvaluation.OrienteEtudiantsRessourcesPertinentes +
                          (int)ficheEvaluation.MontreImportanceSujetTraite +
                          (int)ficheEvaluation.ProdigueEncouragementsFeedback +
                          (int)ficheEvaluation.DemontreInteretRecherche;

            return Math.Round(somme / totalCriteres, 2);
        }

        private bool FicheEvaluationEncadreurExists(int id)
        {
            return _context.FichesEvaluationEncadreur.Any(e => e.Id == id);
        }
    }
}