using StageManager.DTOs;
using StageManager.Models;

namespace StageManager.Mapping
{
    public static class FicheEvaluationEncadreurMapping
    {
        // Conversion de FicheEvaluationEncadreur vers FicheEvaluationEncadreurReadDto
        public static FicheEvaluationEncadreurReadDto ToReadDto(this FicheEvaluationEncadreur fiche)
        {
            return new FicheEvaluationEncadreurReadDto
            {
                Id = fiche.Id,
                DateCreation = fiche.DateCreation,
                NomPrenomEncadreur = fiche.NomPrenomEncadreur,
                FonctionEncadreur = fiche.FonctionEncadreur,
                DateDebutStage = fiche.DateDebutStage,
                DateFinStage = fiche.DateFinStage,
                StageId = fiche.StageId,

                // Catégorie 1: Planification du travail
                FixeObjectifsClairs = fiche.FixeObjectifsClairs,
                GereImprevus = fiche.GereImprevus,
                RencontreRegulierementEtudiants = fiche.RencontreRegulierementEtudiants,
                OrganiseEtapesRecherche = fiche.OrganiseEtapesRecherche,

                // Catégorie 2: Comprendre et faire comprendre
                ExpliqueClairementContenu = fiche.ExpliqueClairementContenu,
                InterrogeEtudiantsFeedback = fiche.InterrogeEtudiantsFeedback,
                MaitriseConnaissances = fiche.MaitriseConnaissances,
                EnseigneFaitDemonstrations = fiche.EnseigneFaitDemonstrations,

                // Catégorie 3: Susciter la participation
                InviteEtudiantsQuestions = fiche.InviteEtudiantsQuestions,
                RepondQuestionsEtudiants = fiche.RepondQuestionsEtudiants,
                EncourageInitiativesEtudiants = fiche.EncourageInitiativesEtudiants,
                InterrogeEtudiantsTravailEffectue = fiche.InterrogeEtudiantsTravailEffectue,
                AccepteExpressionPointsVueDifferents = fiche.AccepteExpressionPointsVueDifferents,

                // Catégorie 4: Communication orale
                CommuniqueClairementSimplement = fiche.CommuniqueClairementSimplement,
                CritiqueConstructive = fiche.CritiqueConstructive,
                PondereQuantiteInformation = fiche.PondereQuantiteInformation,

                // Catégorie 5: Sens de responsabilité
                EfficaceGestionSupervision = fiche.EfficaceGestionSupervision,
                MaintientAttitudeProfessionnelle = fiche.MaintientAttitudeProfessionnelle,
                TransmetDonneesFiables = fiche.TransmetDonneesFiables,

                // Catégorie 6: Stimuler la motivation des étudiants
                OrienteEtudiantsRessourcesPertinentes = fiche.OrienteEtudiantsRessourcesPertinentes,
                MontreImportanceSujetTraite = fiche.MontreImportanceSujetTraite,
                ProdigueEncouragementsFeedback = fiche.ProdigueEncouragementsFeedback,
                DemontreInteretRecherche = fiche.DemontreInteretRecherche,

                Observations = fiche.Observations,
                NomPrenomStagiaireEvaluateur = fiche.NomPrenomStagiaireEvaluateur,
                DateEvaluation = fiche.DateEvaluation,
                EstValide = fiche.EstValide,

                // Informations liées
                Stagiaire = fiche.Stagiaire != null ? new StagiaireMinimalDto
                {
                    Id = fiche.Stagiaire.Id,
                    NomPrenom = $"{fiche.Stagiaire.Nom} {fiche.Stagiaire.Prenom}"
                } : null,
                Encadreur = fiche.Encadreur != null ? new EncadreurMinimalDto
                {
                    Id = fiche.Encadreur.Id,
                    NomPrenom = $"{fiche.Encadreur.Nom} {fiche.Encadreur.Prenom}"
                } : null,
                Stage = fiche.Stage != null ? new StageMinimalDto
                {
                    Id = fiche.Stage.Id,
                } : null
            };
        }

        // Conversion de FicheEvaluationEncadreur vers FicheEvaluationEncadreurListDto
        public static FicheEvaluationEncadreurListDto ToListDto(this FicheEvaluationEncadreur fiche)
        {
            // Calculer le score moyen
            double scoreMoyen = CalculerScoreMoyen(fiche);

            return new FicheEvaluationEncadreurListDto
            {
                Id = fiche.Id,
                NomPrenomEncadreur = fiche.NomPrenomEncadreur,
                FonctionEncadreur = fiche.FonctionEncadreur,
                DateEvaluation = fiche.DateEvaluation,
                NomPrenomStagiaireEvaluateur = fiche.NomPrenomStagiaireEvaluateur,
                StagiaireId = fiche.StagiaireId,
                StageId = fiche.StageId,
                ScoreMoyen = scoreMoyen,
                EstValide = fiche.EstValide
            };
        }

        // Méthode auxiliaire pour calculer le score moyen
        private static double CalculerScoreMoyen(FicheEvaluationEncadreur fiche)
        {
            // Ici, calculez la moyenne de toutes les évaluations
            int total = (int)fiche.FixeObjectifsClairs +
                        (int)fiche.GereImprevus +
                        (int)fiche.RencontreRegulierementEtudiants +
                        (int)fiche.OrganiseEtapesRecherche +
                        (int)fiche.ExpliqueClairementContenu +
                        (int)fiche.InterrogeEtudiantsFeedback +
                        (int)fiche.MaitriseConnaissances +
                        (int)fiche.EnseigneFaitDemonstrations +
                        (int)fiche.InviteEtudiantsQuestions +
                        (int)fiche.RepondQuestionsEtudiants +
                        (int)fiche.EncourageInitiativesEtudiants +
                        (int)fiche.InterrogeEtudiantsTravailEffectue +
                        (int)fiche.AccepteExpressionPointsVueDifferents +
                        (int)fiche.CommuniqueClairementSimplement +
                        (int)fiche.CritiqueConstructive +
                        (int)fiche.PondereQuantiteInformation +
                        (int)fiche.EfficaceGestionSupervision +
                        (int)fiche.MaintientAttitudeProfessionnelle +
                        (int)fiche.TransmetDonneesFiables +
                        (int)fiche.OrienteEtudiantsRessourcesPertinentes +
                        (int)fiche.MontreImportanceSujetTraite +
                        (int)fiche.ProdigueEncouragementsFeedback +
                        (int)fiche.DemontreInteretRecherche;

            return Math.Round((double)total / 23, 2); // 23 critères d'évaluation
        }
    }
}