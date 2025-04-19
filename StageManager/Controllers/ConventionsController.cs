using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
                .Where(c => c.StageId == stageId)
                .ToListAsync();

            return conventions.Select(c => c.ToDto()).ToList();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConvention(int id, UpdateConventionDto updateConventionDto)
        {
            var convention = await _context.Conventions
                .Include(c => c.DemandeAccord)
                .ThenInclude(d => d.stagiaires)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (convention == null)
            {
                return NotFound();
            }

            var membreDirection = await _context.MembresDirection.FindAsync(updateConventionDto.MembreDirectionId);
            if (membreDirection == null)
            {
                return BadRequest("Le membre de direction spécifié n'existe pas.");
            }

            // Vérifier si StageId est fourni et existe
            if (updateConventionDto.StageId.HasValue)
            {
                var stage = await _context.Stages.FindAsync(updateConventionDto.StageId.Value);
                if (stage == null)
                {
                    return BadRequest("Le stage spécifié n'existe pas.");
                }
                convention.StageId = updateConventionDto.StageId;
            }

            // Sauvegarde de l'ancien statut pour comparaison
            var oldStatus = convention.status;

            // Mise à jour des propriétés de base
            convention.CheminFichier = updateConventionDto.CheminFichier;
            convention.MembreDirectionId = updateConventionDto.MembreDirectionId;
            convention.status = updateConventionDto.Status;

            // Logique spécifique si le statut a changé à Accepté
            if (oldStatus != Statusconvention.Accepte && convention.status == Statusconvention.Accepte)
            {
                // Créer un nouveau Stage associé à cette convention
                var demandeAccord = convention.DemandeAccord;

                var newStage = new Stage
                {
                    StagiaireGroup = $"Groupe-{demandeAccord.Id}",
                    DateDebut = demandeAccord.DateDebut ?? DateTime.Now,
                    DateFin = demandeAccord.DateFin ?? DateTime.Now.AddMonths(3),
                    Statut = StatutStage.EnCours,
                    ConventionId = convention.Id,
                    DepartementId = demandeAccord.ChefDepartement?.DepartementId ?? 1, // Utiliser un département par défaut si non spécifié
                    EncadreurId = demandeAccord.EncadreurId ?? 1, // Utiliser un encadreur par défaut si non spécifié
                    Stagiaires = new List<Stagiaire>()
                };

                // Ajouter les stagiaires de la demande d'accord au stage
                if (demandeAccord.stagiaires != null && demandeAccord.stagiaires.Any())
                {
                    foreach (var stagiaire in demandeAccord.stagiaires)
                    {
                        stagiaire.Status = StagiaireStatus.Accepte; // Assurez-vous que cette propriété existe dans votre modèle Stagiaire
                        newStage.Stagiaires.Add(stagiaire);

                        // Envoyer email de confirmation du début de stage
                        string sujet = "Confirmation du début de votre stage";
                        string corps = $"Bonjour {stagiaire.Nom} {stagiaire.Prenom},\n\n" +
                                      $"Nous avons le plaisir de vous informer que votre convention de stage a été acceptée. " +
                                      $"Votre stage débutera le {newStage.DateDebut.ToString("dd/MM/yyyy")} et se terminera le {newStage.DateFin.ToString("dd/MM/yyyy")}.\n\n" +
                                      $"Veuillez vous présenter au service d'accueil à la date de début pour finaliser les formalités administratives.\n\n" +
                                      $"Cordialement,\nService des Stages";

                        await EnvoyerEmailAsync(stagiaire.Email, sujet, corps);
                    }
                }

                _context.Stages.Add(newStage);

                // Mettre à jour l'ID du stage dans la convention
                convention.StageId = newStage.Id;
            }
            // Logique spécifique si le statut a changé à Refusé
            else if (oldStatus != Statusconvention.Refuse && convention.status == Statusconvention.Refuse)
            {
                // Mettre à jour le statut des stagiaires à "Refuse"
                if (convention.DemandeAccord?.stagiaires != null && convention.DemandeAccord.stagiaires.Any())
                {
                    foreach (var stagiaire in convention.DemandeAccord.stagiaires)
                    {
                        stagiaire.Status = StagiaireStatus.Refuse; // Assurez-vous que cette propriété existe dans votre modèle Stagiaire
                        stagiaire.EstActif = false;  // Désactiver le compte stagiaire

                        // Envoyer email de refus
                        string sujet = "Refus de votre demande de stage";
                        string corps = $"Bonjour {stagiaire.Nom} {stagiaire.Prenom},\n\n" +
                                      $"Nous regrettons de vous informer que votre convention de stage a été refusée. " +
                                      $"Par conséquent, votre compte a été désactivé sur notre plateforme.\n\n" +
                                      $"Pour toute information complémentaire, veuillez nous contacter par email.\n\n" +
                                      $"Cordialement,\nService des Stages";

                        await EnvoyerEmailAsync(stagiaire.Email, sujet, corps);
                    }
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