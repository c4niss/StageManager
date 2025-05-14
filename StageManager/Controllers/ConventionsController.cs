using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using StageManager.DTO.ConventionDTO;
using StageManager.Mapping;
using StageManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;
using static StageManager.Models.Convention;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConventionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ConventionController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Convention
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConventionDto>>> GetConventions()
        {
            var conventions = await _context.Conventions
                .Include(c => c.Stage)
                .Include(c => c.MembreDirection)
                .Include(c => c.DemandeAccord)
                    .ThenInclude(d => d.stagiaires)
                .Include(c => c.DemandeAccord)
                    .ThenInclude(d => d.Theme)
                .ToListAsync();
            return conventions.Select(c => c.ToDto()).ToList();
        }

        // GET: api/Convention/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConventionDto>> GetConvention(int id)
        {
            var convention = await _context.Conventions
                .Include(c => c.Stage)
                .Include(c => c.MembreDirection)
                .Include(c => c.DemandeAccord)
                    .ThenInclude(d => d.stagiaires)
                .Include(c => c.DemandeAccord)
                    .ThenInclude(d => d.Theme)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (convention == null)
            {
                return NotFound();
            }

            return convention.ToDto();
        }

        // GET: api/Convention/ByStage/5
        [HttpGet("ByStage/{stageId}")]
        public async Task<ActionResult<IEnumerable<ConventionDto>>> GetConventionsByStage(int stageId)
        {
            var conventions = await _context.Conventions
                .Include(c => c.Stage)
                .Include(c => c.MembreDirection)
                .Include(c => c.DemandeAccord)
                .Where(c => c.StageId == stageId)
                .ToListAsync();

            return conventions.Select(c => c.ToDto()).ToList();
        }

        // PUT: api/Convention/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConvention(int id, UpdateConventionDto updateConventionDto)
        {
            var convention = await _context.Conventions
                .Include(c => c.DemandeAccord)
                    .ThenInclude(d => d.stagiaires)
                .Include(c => c.Stage)
                .Include(c => c.DemandeAccord)
                    .ThenInclude(d => d.Theme)
                .Include(c => c.DemandeAccord)
                    .ThenInclude(d => d.Encadreur)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (convention == null)
            {
                return NotFound();
            }

            // Sauvegarde de l'ancien statut pour comparaison
            var oldStatus = convention.status;

            // Mise à jour des propriétés de base
            convention.CheminFichier = updateConventionDto.CheminFichier;
            convention.status = updateConventionDto.Status;
            convention.Commentaire = updateConventionDto.Commentaire;
            // Logique spécifique si le statut a changé à Accepté
            if (oldStatus != Statusconvention.Accepte && convention.status == Statusconvention.Accepte)
            {
                // Mettre à jour le statut de la demande d'accord
                if (convention.DemandeAccord != null)
                {
                    convention.DemandeAccord.Status = StatusAccord.Accepte;

                    // Mettre à jour le statut des stagiaires
                    if (convention.DemandeAccord.stagiaires != null)
                    {
                        foreach (var stagiaire in convention.DemandeAccord.stagiaires)
                        {
                            stagiaire.Status = StagiaireStatus.Accepte;

                            // Créer une fiche de pointage pour chaque stagiaire
                            var fichePointage = new FicheDePointage
                            {
                                DateCreation = DateTime.Now,
                                NomPrenomStagiaire = $"{stagiaire.Nom} {stagiaire.Prenom}",
                                StructureAccueil = convention.DemandeAccord.ServiceAccueil ?? "Non spécifié",
                                NomQualitePersonneChargeSuivi = $"{convention.DemandeAccord.Encadreur?.Nom} {convention.DemandeAccord.Encadreur?.Prenom}",
                                DateDebutStage = convention.Stage.DateDebut,
                                DateFinStage = convention.Stage.DateFin,
                                NatureStage = convention.DemandeAccord.NatureStage ?? NatureStage.StageImpregnation,
                                DonneesPointage = "",
                                EstValide = false,
                                StagiaireId = stagiaire.Id,
                                EncadreurId = convention.Stage.EncadreurId,
                                StageId = convention.Stage.Id
                            };

                            _context.FichesDePointage.Add(fichePointage);

                            // Créer une fiche d'évaluation individuelle pour ce stagiaire
                            var ficheEvaluationStagiaire = new FicheEvaluationStagiaire
                            {
                                DateCreation = DateTime.Now,
                                NomPrenomStagiaire = $"{stagiaire.Nom} {stagiaire.Prenom}",
                                FormationStagiaire = stagiaire.Specialite,
                                DureeStage = (convention.Stage.DateFin - convention.Stage.DateDebut).Days.ToString() + " jours",
                                PeriodeDu = convention.Stage.DateDebut,
                                PeriodeAu = convention.Stage.DateFin,
                                StructureAccueil = convention.DemandeAccord.ServiceAccueil ?? "Non spécifié",
                                NombreSeancesPrevues = convention.DemandeAccord.NombreSeancesParSemaine ?? 0,
                                NomPrenomEncadreur = $"{convention.DemandeAccord.Encadreur?.Nom} {convention.DemandeAccord.Encadreur?.Prenom}",
                                FonctionEncadreur = convention.DemandeAccord.Encadreur?.Fonction ?? "Non spécifié",
                                ThemeStage = convention.DemandeAccord.Theme?.Nom ?? "Non spécifié",
                                MissionsConfieesAuStagiaire = "À compléter",
                                NomPrenomEvaluateur = "",
                                DateEvaluation = DateTime.Now,
                                EncadreurId = convention.Stage.EncadreurId,
                                StagiaireId = stagiaire.Id,
                                StageId = convention.Stage.Id
                            };
                            _context.FichesEvaluationStagiaire.Add(ficheEvaluationStagiaire);
                            var ficheEvaluationEncadreur = new FicheEvaluationEncadreur
                            {
                                DateCreation = DateTime.Now,
                                NomPrenomEncadreur = $"{convention.DemandeAccord.Encadreur?.Nom} {convention.DemandeAccord.Encadreur?.Prenom}",
                                FonctionEncadreur = convention.DemandeAccord.Encadreur?.Fonction ?? "Non spécifié",
                                DateDebutStage = convention.Stage.DateDebut,
                                DateFinStage = convention.Stage.DateFin,
                                Observations = "",
                                NomPrenomStagiaireEvaluateur = $"{stagiaire.Nom} {stagiaire.Prenom}",
                                DateEvaluation = DateTime.Now,
                                EncadreurId = convention.Stage.EncadreurId,
                                StagiaireId = stagiaire.Id,
                                StageId = convention.Stage.Id
                            };
                            _context.FichesEvaluationEncadreur.Add(ficheEvaluationEncadreur);

                            // Envoyer email de confirmation
                            string sujet = "Confirmation du début de votre stage";
                            string corps = $"Bonjour {stagiaire.Nom} {stagiaire.Prenom},\n\n" +
                                $"Nous avons le plaisir de vous informer que votre convention de stage a été acceptée. " +
                                $"Votre stage débutera le {convention.Stage?.DateDebut.ToString("dd/MM/yyyy")} et se terminera le {convention.Stage?.DateFin.ToString("dd/MM/yyyy")}.\n\n" +
                                $"Veuillez vous présenter au service d'accueil à la date de début pour finaliser les formalités administratives.\n\n" +
                                $"Cordialement,\nService des Stages";

                            await EnvoyerEmailAsync(stagiaire.Email, sujet, corps);
                        }
                    }
                    
                }

                // Mettre à jour le statut du stage s'il existe
                if (convention.Stage != null)
                {
                    convention.Stage.Statut = StatutStage.EnCours;
                }
            }
            // Logique spécifique si le statut a changé à Refusé
            else if (oldStatus != Statusconvention.Refuse && convention.status == Statusconvention.Refuse)
            {
                // Mettre à jour le statut des stagiaires à "Refuse"
                if (convention.DemandeAccord?.stagiaires != null)
                {
                    foreach (var stagiaire in convention.DemandeAccord.stagiaires)
                    {
                        stagiaire.Status = StagiaireStatus.Refuse;

                        // Envoyer email de refus
                        string sujet = "Refus de votre demande de stage";
                        string corps = $"Bonjour {stagiaire.Nom} {stagiaire.Prenom},\n\n" +
                            $"Nous regrettons de vous informer que votre convention de stage a été refusée. " +
                            $"Pour toute information complémentaire, veuillez nous contacter par email.\n\n" +
                            $"Cordialement,\nService des Stages";

                        await EnvoyerEmailAsync(stagiaire.Email, sujet, corps);
                    }

                    // Mettre à jour le statut de la demande d'accord
                    convention.DemandeAccord.Status = StatusAccord.Refuse;
                }

                // Mettre à jour le statut du stage s'il existe
                if (convention.Stage != null)
                {
                    convention.Stage.Statut = StatutStage.Annule;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConventionExists(id))
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

        // DELETE: api/Convention/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConvention(int id)
        {
            var convention = await _context.Conventions.FindAsync(id);
            if (convention == null)
            {
                return NotFound();
            }

            _context.Conventions.Remove(convention);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task EnvoyerEmailAsync(string destinataire, string sujet, string corps)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Service des Stages", _config["Email:From"]));
            email.To.Add(new MailboxAddress("", destinataire));
            email.Subject = sujet;
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = corps
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(
                _config["Email:Host"],
                int.Parse(_config["Email:Port"]),
                SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                _config["Email:Username"],
                _config["Email:Password"]);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }
        [HttpPost("upload")]
        [Authorize(Roles = "MembreDirection")]
        public async Task<ActionResult<string>> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier n'a été téléchargé.");

            if (file.Length > 512000)
                return BadRequest("La taille du fichier dépasse 500KB.");

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Format de fichier non supporté. Veuillez utiliser PDF, DOC, DOCX, JPG ou PNG.");

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                return Ok($"{baseUrl}/uploads/{uniqueFileName}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors du téléchargement du fichier: {ex.Message}");
            }
        }

        [HttpGet("download/{fileName}")]
        [Authorize(Roles = "MembreDirection")]
        public IActionResult DownloadFile(string fileName)
        {
            try
            {
                fileName = Path.GetFileName(fileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Le fichier demandé n'existe pas.");
                }

                var contentType = GetContentType(fileName);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors du téléchargement du fichier: {ex.Message}");
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
        private string GetAccordFormLink(int accordId)
        {
            return $"{_config["Frontend:BaseUrl"]}/accord/{accordId}/completer";
        }

        private bool ConventionExists(int id)
        {
            return _context.Conventions.Any(e => e.Id == id);
        }
    }
}