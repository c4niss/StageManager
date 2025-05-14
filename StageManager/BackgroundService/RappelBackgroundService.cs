using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using StageManager.Models;
using TestRestApi.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.EntityFrameworkCore;

namespace StageManager.BackgroundService
{
    public class RappelBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<RappelBackgroundService> _logger;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;

        public RappelBackgroundService(
            ILogger<RappelBackgroundService> logger,
            IServiceProvider services,
            IConfiguration config)
        {
            _logger = logger;
            _services = services;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service de rappel démarré");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    await ProcesserRappels(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du traitement des rappels");
                }

                // Vérification toutes les secondes
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private async Task ProcesserRappels(AppDbContext context)
        {
            // Modifié pour inclure les demandes qui sont refusées mais n'ont pas encore reçu le rappel du jour 7
            var demandes = await context.DemandesAccord
                .Where(d => (d.Status == StatusAccord.EnCours ||
                            (d.Status == StatusAccord.Refuse && !d.RappelJour7Envoye))
                         && d.DateSoumissionStagiaire.HasValue
                         && d.ChefDepartementId != null)
                .Include(d => d.ChefDepartement)
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .ToListAsync();

            foreach (var demande in demandes)
            {
                var delai = (DateTime.Now - demande.DateSoumissionStagiaire.Value).TotalSeconds;

                if (delai >= 60 && delai < 65 && !demande.RappelJour6Envoye)
                {
                    bool emailEnvoye = await EnvoyerRappelChefDepartement(
                        demande,
                        $"Rappel: Demande n°{demande.Id} en attente",
                        $"Bonjour {demande.ChefDepartement.Prenom},\n\n" +
                        "Merci de traiter cette demande dans les plus brefs délais.\n" +
                        "Délai restant : 24h\n\nCordialement,\nLe service des stages");

                    if (emailEnvoye)
                    {
                        demande.RappelJour6Envoye = true;
                        _logger.LogInformation($"Rappel jour 6 marqué comme envoyé pour la demande {demande.Id}");
                    }
                }
                else if (delai >= 65 && !demande.RappelJour7Envoye)
                {
                    bool emailEnvoye = await EnvoyerRappelChefDepartement(
                        demande,
                        $"URGENT: Demande n°{demande.Id} expirée",
                        $"Bonjour {demande.ChefDepartement.Prenom},\n\n" +
                        "Cette demande a été automatiquement rejetée après 7 jours.\n\n" +
                        "Cordialement,\nLe service des stages");

                    if (emailEnvoye)
                    {
                        demande.Status = StatusAccord.Refuse;
                        demande.RappelJour7Envoye = true;
                        _logger.LogInformation($"Demande {demande.Id} marquée comme refusée et rappel jour 7 envoyé");
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private async Task<bool> EnvoyerRappelChefDepartement(Demandeaccord demande, string sujet, string corps)
        {
            if (demande.ChefDepartement?.Email == null)
            {
                _logger.LogWarning($"Aucun chef de département assigné pour la demande {demande.Id}");
                return false;
            }

            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Service Stages", _config["Email:From"]));
                email.To.Add(MailboxAddress.Parse(demande.ChefDepartement.Email));
                email.Subject = sujet;
                email.Body = new TextPart(TextFormat.Plain) { Text = corps };

                using var client = new SmtpClient();

                // Configuration SMTP
                await client.ConnectAsync(
                    _config["Email:Host"],
                    int.Parse(_config["Email:Port"]),
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    _config["Email:Username"],
                    _config["Email:Password"]);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email envoyé à {demande.ChefDepartement.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Échec d'envoi à {Email}", demande.ChefDepartement.Email);
                return false;
            }
        }
    }
}