using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DemandeaccordDTO;
using StageManager.Mapping;
using StageManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using StageManager.BackgroundService;
using System.Security.Claims;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeaccordController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DemandeaccordController> _logger;

        public DemandeaccordController(AppDbContext context, IConfiguration config, IServiceProvider serviceProvider, ILogger<DemandeaccordController> logger)
        {
            _context = context;
            _config = config;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeaccordDto>>> GetDemandesAccord()
        {
            var demandes = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .Include(d => d.ChefDepartement)
                .ToListAsync();
            return demandes.Select(d => DemandeAccordMapping.ToDto(d)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DemandeaccordDto>> GetDemandeaccord(int id)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null) return NotFound();
            return DemandeAccordMapping.ToDto(demandeaccord);
        }
        [HttpGet("ByChefDepartement/{id}")]
        public async Task<ActionResult<IEnumerable<DemandeaccordDto>>> GetDemandesByChefDepartement(int id)
        {
            // Vérifier que le chef de département existe
            var chefExists = await _context.ChefDepartements.AnyAsync(c => c.Id == id);
            if (!chefExists)
            {
                return NotFound("Chef de département non trouvé");
            }

            // Pour les chefs de département, on vérifie que l'utilisateur courant est bien le chef demandé
            if (User.IsInRole("ChefDepartement"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int currentUserId) || currentUserId != id)
                {
                    return Forbid("Vous n'êtes pas autorisé à accéder aux demandes d'un autre chef de département");
                }
            }

            var demandes = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .Where(d => d.ChefDepartementId == id)
                .ToListAsync();

            return demandes.Select(d => DemandeAccordMapping.ToDto(d)).ToList();
        }

        [HttpPut("MembreDirectionUpdate/{id}")]
        public async Task<IActionResult> UpdateMembreDirectionDemandeaccord(int id, UpdatemembreDirectionDemandeaccorDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null) return NotFound();
            var departement = await _context.Departements.FindAsync(updateDto.DepartementId);
            if (departement == null) return BadRequest("Département inexistant.");
            var domaine = await _context.Domaines.FindAsync(updateDto.DomaineId);
            if (domaine == null) return BadRequest("Domaine inexistant.");
            int? stageId = demandeaccord.stagiaires.FirstOrDefault(s => s.StageId != null)?.StageId;
            var theme = await _context.Themes
                .FirstOrDefaultAsync(t =>
                    t.Nom == updateDto.ThemeNom &&
                    t.DepartementId == updateDto.DepartementId &&
                    t.DomaineId == updateDto.DomaineId &&
                    t.StageId == stageId);
            if (theme == null)
            {
                theme = new Theme
                {
                    Nom = updateDto.ThemeNom,
                    DepartementId = updateDto.DepartementId,
                    DomaineId = updateDto.DomaineId,
                    DemandeaccordId = demandeaccord.Id,
                    StageId = stageId
                };
                _context.Themes.Add(theme);
                await _context.SaveChangesAsync();
            }
            demandeaccord.ThemeId = theme.Id;
            demandeaccord.NatureStage = updateDto.NatureStage;
            demandeaccord.UniversiteInstitutEcole = updateDto.UniversiteInstitutEcole;
            demandeaccord.FiliereSpecialite = updateDto.FiliereSpecialite;
            demandeaccord.DiplomeObtention = updateDto.DiplomeObtention;
            demandeaccord.DateSoumissionStagiaire = DateTime.Now;
            if (!IsMembreDirectionPartComplete(demandeaccord))
            {
                return BadRequest("La partie stagiaire n'est pas complète");
            }
            await _context.SaveChangesAsync();
            StartRappelService();
            if (demandeaccord.ChefDepartementId.HasValue)
            {
                var chef = await _context.ChefDepartements.FindAsync(demandeaccord.ChefDepartementId);
                if (chef != null)
                {
                    await EnvoyerEmailAsync(
                        chef.Email,
                        "Nouvelle demande à valider",
                        $"Bonjour {chef.Prenom},\n\n" +
                        $"Une nouvelle demande d'accord (n°{demandeaccord.Id}) a été soumise par le stagiaire.\n" +
                        "Merci de la traiter dans les délais.\n\n" +
                        "Cordialement,\nLe service des stages");
                }
            }
            StartRappelService();
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DemandesAccord.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpPut("ChefDepartementUpdate/{id}")]
        public async Task<IActionResult> UpdateChefDepartementDemandeaccord(int id, UpdateChefDepartementDemandeaccordDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null) return NotFound();
            if (updateDto.DateFin <= updateDto.DateDebut) return BadRequest("La date de fin doit être postérieure à la date de début.");
            demandeaccord.ServiceAccueil = updateDto.ServiceAccueil;
            demandeaccord.DateDebut = updateDto.DateDebut;
            demandeaccord.DateFin = updateDto.DateFin;
            demandeaccord.NombreSeancesParSemaine = updateDto.NombreSeancesParSemaine;
            demandeaccord.DureeSeances = updateDto.DureeSeances;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateDemandeaccordStatusDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null) return NotFound();
            if (!IsDemandeaccordComplete(demandeaccord)) return BadRequest("La demande d'accord doit être complète avant de changer le statut.");
            if (demandeaccord.DateFin <= demandeaccord.DateDebut) return BadRequest("La date de fin doit être postérieure à la date de début.");
            var oldStatus = demandeaccord.Status;
            demandeaccord.Status = updateDto.Status;
            demandeaccord.commentaire = updateDto.Commentaire;
            var demandedestage = await _context.DemandesDeStage
                .Include(d => d.MembreDirection)
                .FirstOrDefaultAsync(d => d.Id == demandeaccord.DemandeStageId);
            int? membreDirectionId = demandedestage?.MembreDirectionId;
            if (membreDirectionId == null) return BadRequest("Aucun membre de direction associé à la demande de stage.");
            try
            {
                await _context.SaveChangesAsync();
                if (demandeaccord.Status == StatusAccord.Accepte)
                {
                    var existingconvention = await _context.Conventions
                        .FirstOrDefaultAsync(a => a.DemandeAccordId == demandeaccord.Id);
                    Convention convention;
                    if (existingconvention == null)
                    {
                        convention = new Convention
                        {
                            DemandeAccordId = demandeaccord.Id,
                            status = Convention.Statusconvention.EnCours,
                            DemandeAccord = demandeaccord,
                            MembreDirectionId = membreDirectionId.Value
                        };
                        _context.Conventions.Add(convention);
                        await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE DemandesAccord SET conventionId = {0} WHERE Id = {1}",
                        convention.Id, demandeaccord.Id);
                        //await _context.SaveChangesAsync();

                    }
                    else convention = existingconvention;
                    int? stageId = demandeaccord.stagiaires.FirstOrDefault(s => s.StageId != null)?.StageId;
                    if (stageId.HasValue)
                    {
                        var stage = await _context.Stages.FirstOrDefaultAsync(s => s.Id == stageId.Value);
                        if (stage != null)
                        {
                            stage.DateDebut = demandeaccord.DateDebut ?? stage.DateDebut;
                            stage.DateFin = demandeaccord.DateFin ?? stage.DateFin;
                            stage.DepartementId = demandeaccord.Theme?.DepartementId ?? stage.DepartementId;
                            stage.DomaineId = demandeaccord.Theme?.DomaineId ?? stage.DomaineId;
                            stage.Statut = StatutStage.EnCours;
                            stage.EncadreurId = demandeaccord.EncadreurId ?? stage.EncadreurId;
                            stage.ConventionId = convention.Id;
                            convention.StageId = stageId;
                            await _context.SaveChangesAsync();
                        }
                    }
                    convention.StageId = stageId;

                    foreach (var stagiaire in demandeaccord.stagiaires)
                    {
                        await EnvoyerEmailAsync(
                            stagiaire.Email,
                            "Demande d'accord de stage acceptée",
                            $"Bonjour {stagiaire.Prenom},\n\n" +
                            $"Votre demande d'accord de stage n°{demandeaccord.Id} a été acceptée.\n" +
                            "Nous vous prions de bien vouloir déposer votre convention auprès de la direction.\n\n" +
                            $"Votre numéro de convention est {convention.Id}\n" +
                            "Cordialement,\nLe service des stages");
                    }
                }
                else if (demandeaccord.Status == StatusAccord.Refuse)
                {
                    foreach (var stagiaire in demandeaccord.stagiaires)
                    {
                        await EnvoyerEmailAsync(
                            stagiaire.Email,
                            "Demande d'accord de stage refusée",
                            $"Bonjour {stagiaire.Prenom},\n\n" +
                            "Nous regrettons de vous informer que votre demande d'accord de stage n'a pas été retenue.\n" +
                            $"{updateDto.Commentaire}" +
                            "Cordialement,\nLe service des stages");
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPut("AssignEncadreur/{id}")]
        public async Task<IActionResult> AssignEncadreur(int id, [FromQuery] int encadreurId)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null) return NotFound();
            var encadreur = await _context.Encadreurs.FindAsync(encadreurId);
            if (encadreur == null) return BadRequest("L'encadreur spécifié n'existe pas.");
            demandeaccord.EncadreurId = encadreurId;
            try
            {
                await _context.SaveChangesAsync();
                string listeStagiaires = string.Join(", ", demandeaccord.stagiaires.Select(s => $"{s.Nom} {s.Prenom}"));
                string theme = demandeaccord.Theme != null ? demandeaccord.Theme.Nom : "Non spécifié";
                await EnvoyerEmailAsync(
                    encadreur.Email,
                    "Assignation d'encadrement de stage",
                    $"Bonjour {encadreur.Nom} {encadreur.Prenom},\n\n" +
                    $"Vous avez été assigné(e) comme encadreur pour la demande d'accord de stage n°{demandeaccord.Id}.\n\n" +
                    $"Stagiaire(s) : {listeStagiaires}\n" +
                    $"Thème : {theme}\n\n" +
                    "Pour plus d'informations, veuillez consulter votre espace personnel.\n\n" +
                    "Cordialement,\nLe service des stages");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPut("AssignChefDepartement/{id}")]
        public async Task<IActionResult> AssignChefDepartement(int id, [FromQuery] int chefDepartementId)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null) return NotFound();
            var chefDepartement = await _context.ChefDepartements
                .Include(c => c.Departement)
                .FirstOrDefaultAsync(c => c.Id == chefDepartementId);
            if (chefDepartement == null) return BadRequest("Le chef de département spécifié n'existe pas.");
            demandeaccord.ChefDepartementId = chefDepartementId;
            try
            {
                await _context.SaveChangesAsync();
                string listeStagiaires = string.Join(", ", demandeaccord.stagiaires.Select(s => $"{s.Nom} {s.Prenom}"));
                string theme = demandeaccord.Theme != null ? demandeaccord.Theme.Nom : "Non spécifié";
                await EnvoyerEmailAsync(
                    chefDepartement.Email,
                    "Assignation de demande de stage",
                    $"Bonjour {chefDepartement.Nom} {chefDepartement.Prenom},\n\n" +
                    $"Une nouvelle demande d'accord de stage n°{demandeaccord.Id} vous a été assignée.\n\n" +
                    $"Stagiaire(s) : {listeStagiaires}\n" +
                    $"Thème : {theme}\n" +
                    $"Département : {chefDepartement.Departement?.Nom}\n\n" +
                    "Pour plus d'informations, veuillez consulter votre espace personnel.\n\n" +
                    "Cordialement,\nLe service des stages");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDemandeacoord(int id)
        {
            var demandeaccord = await _context.DemandesAccord.FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null) return NotFound();
            _context.DemandesAccord.Remove(demandeaccord);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DemandeaccordExists(int id) => _context.DemandesAccord.Any(e => e.Id == id);

        private void StartRappelService()
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var rappelService = _serviceProvider.GetService<IHostedService>() as RappelBackgroundService;
                if (rappelService == null)
                {
                    _logger.LogInformation("Démarrage du service de rappel");
                    var backgroundService = ActivatorUtilities.CreateInstance<RappelBackgroundService>(
                        scopedServices,
                        scopedServices.GetRequiredService<ILogger<RappelBackgroundService>>(),
                        _serviceProvider,
                        _config);
                    Task.Run(() => backgroundService.StartAsync(CancellationToken.None));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du démarrage du service de rappel");
            }
        }

        private async Task EnvoyerEmailAsync(string destinataire, string sujet, string corps)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Service des Stages", _config["Email:From"]));
            email.To.Add(new MailboxAddress("", destinataire));
            email.Subject = sujet;
            email.Body = new TextPart(TextFormat.Plain) { Text = corps };
            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_config["Email:Host"], int.Parse(_config["Email:Port"]), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }
        private bool IsMembreDirectionPartComplete(Demandeaccord demandeaccord) =>
            !string.IsNullOrWhiteSpace(demandeaccord.UniversiteInstitutEcole) &&
            !string.IsNullOrWhiteSpace(demandeaccord.FiliereSpecialite) &&
            demandeaccord.ThemeId != null &&
            !string.IsNullOrWhiteSpace(demandeaccord.DiplomeObtention) &&
            demandeaccord.NatureStage != null;

        private bool IsDemandeaccordComplete(Demandeaccord demandeaccord) =>
            !string.IsNullOrWhiteSpace(demandeaccord.UniversiteInstitutEcole) &&
            !string.IsNullOrWhiteSpace(demandeaccord.FiliereSpecialite) &&
            demandeaccord.ThemeId != null &&
            !string.IsNullOrWhiteSpace(demandeaccord.DiplomeObtention) &&
            demandeaccord.NatureStage != null &&
            !string.IsNullOrWhiteSpace(demandeaccord.ServiceAccueil) &&
            demandeaccord.DateDebut != default(DateTime) &&
            demandeaccord.DateFin != default(DateTime) &&
            demandeaccord.NombreSeancesParSemaine > 0 &&
            demandeaccord.DureeSeances > 0 &&
            demandeaccord.EncadreurId != null;
    }
}
