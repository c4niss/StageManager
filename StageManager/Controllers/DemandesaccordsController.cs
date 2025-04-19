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

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeaccordController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public DemandeaccordController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Demandeaccord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeaccordDto>>> GetDemandesAccord()
        {
            var demandes = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .ToListAsync();

            return demandes.Select(d => DemandeAccordMapping.ToDto(d)).ToList();
        }

        // GET: api/Demandeaccord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DemandeaccordDto>> GetDemandeaccord(int id)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            return DemandeAccordMapping.ToDto(demandeaccord);
        }
        // PUT: api/Demandeaccord/StagiaireUpdate/5
        [HttpPut("StagiaireUpdate/{id}")]
        public async Task<IActionResult> UpdateStagiaireDemandeaccord(int id, UpdateStagiaireDemandeaccordDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            // Mettre à jour les propriétés du stagiaire
            demandeaccord.UniversiteInstitutEcole = updateDto.UniversiteInstitutEcole;
            demandeaccord.FiliereSpecialite = updateDto.FiliereSpecialite;
            demandeaccord.Telephone = updateDto.Telephone;
            demandeaccord.Email = updateDto.Email;
            demandeaccord.DiplomeObtention = updateDto.DiplomeObtention;
            demandeaccord.Nom = updateDto.Nom;
            demandeaccord.Prenom = updateDto.Prenom;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id))
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
        // PUT: api/Demandeaccord/MembreDirectionUpdate/5
        [HttpPut("MembreDirectionUpdate/{id}")]
        public async Task<IActionResult> UpdateMembreDirectionDemandeaccord(int id, UpdatemembreDirectionDemandeaccorDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes.FindAsync(updateDto.ThemeId);
            if (theme == null)
            {
                return BadRequest("Le thème spécifié n'existe pas.");
            }
            demandeaccord.ThemeId = updateDto.ThemeId;
            demandeaccord.NatureStage = updateDto.NatureStage;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id))
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
        // PUT: api/Demandeaccord/ChefDepartementUpdate/5
        [HttpPut("ChefDepartementUpdate/{id}")]
        public async Task<IActionResult> UpdateChefDepartementDemandeaccord(int id, UpdateChefDepartementDemandeaccordDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            // Vérifier que la date de fin est après la date de début
            if (updateDto.DateFin <= updateDto.DateDebut)
            {
                return BadRequest("La date de fin doit être postérieure à la date de début.");
            }

            // Mettre à jour les propriétés par le chef de département
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
                if (!DemandeaccordExists(id))
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

        // PUT: api/Demandeaccord/UpdateStatus/5
        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateDemandeaccordStatusDto updateDto)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.DemandeDeStage)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            // Validation : Vérification que la demande est complète
            if (!IsDemandeaccordComplete(demandeaccord))
            {
                return BadRequest("La demande d'accord doit être complète avant de changer le statut.");
            }

            // Validation : Vérification des dates
            if (demandeaccord.DateFin <= demandeaccord.DateDebut)
            {
                return BadRequest("La date de fin doit être postérieure à la date de début.");
            }

            var oldStatus = demandeaccord.Status;
            demandeaccord.Status = updateDto.Status;

            try
            {
                await _context.SaveChangesAsync();

                // Envoi d'emails en fonction du statut
                if (demandeaccord.Status == StatusAccord.Accepte)
                {
                    foreach (var stagiaire in demandeaccord.stagiaires)
                    {
                        await EnvoyerEmailAsync(
                            stagiaire.Email,
                            "Demande d'accord de stage acceptée",
                            $"Bonjour {stagiaire.Prenom},\n\n" +
                            $"Votre demande d'accord de stage n°{demandeaccord.Id} a été acceptée.\n" +
                            "Nous vous prions de bien vouloir déposer votre convention auprès de la direction.\n\n" +
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
                            "Pour plus d'informations, vous pouvez contacter le service des stages.\n\n" +
                            "Cordialement,\nLe service des stages");
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeaccordExists(id))
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

        // PUT: api/Demandeaccord/AssignEncadreur/5?encadreurId=10
        [HttpPut("AssignEncadreur/{id}")]
        public async Task<IActionResult> AssignEncadreur(int id, [FromQuery] int encadreurId)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            var encadreur = await _context.Encadreurs.FindAsync(encadreurId);
            if (encadreur == null)
            {
                return BadRequest("L'encadreur spécifié n'existe pas.");
            }

            demandeaccord.EncadreurId = encadreurId;

            try
            {
                await _context.SaveChangesAsync();

                // Envoi d'email à l'encadreur
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
                if (!DemandeaccordExists(id))
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
        // PUT: api/Demandeaccord/AssignChefDepartement/5?chefDepartementId=10
        [HttpPut("AssignChefDepartement/{id}")]
        public async Task<IActionResult> AssignChefDepartement(int id, [FromQuery] int chefDepartementId)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
                .Include(d => d.Theme)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            var chefDepartement = await _context.ChefDepartements
                .Include(c => c.Departement)
                .FirstOrDefaultAsync(c => c.Id == chefDepartementId);

            if (chefDepartement == null)
            {
                return BadRequest("Le chef de département spécifié n'existe pas.");
            }
            demandeaccord.ChefDepartementId = chefDepartementId;
            try
            {
                await _context.SaveChangesAsync();

                // Envoi d'email au chef de département
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
                if (!DemandeaccordExists(id))
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
        [HttpDelete("{id}")]
        // DELETE: api/Demandeaccord/5
        public async Task<IActionResult> DeleteDemandeacoord(int id)
        {
            var demandeaccord = await _context.DemandesAccord.FirstOrDefaultAsync(d => d.Id == id);
            if (demandeaccord == null)
            {
                return NotFound();
            }
            _context.DemandesAccord.Remove(demandeaccord);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DemandeaccordExists(int id)
        {
            return _context.DemandesAccord.Any(e => e.Id == id);
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
        private bool IsDemandeaccordComplete(Demandeaccord demandeaccord)
        {
            return !string.IsNullOrWhiteSpace(demandeaccord.Nom) &&
                   !string.IsNullOrWhiteSpace(demandeaccord.Prenom) &&
                   !string.IsNullOrWhiteSpace(demandeaccord.UniversiteInstitutEcole) &&
                   !string.IsNullOrWhiteSpace(demandeaccord.FiliereSpecialite) &&
                   !string.IsNullOrWhiteSpace(demandeaccord.Telephone) &&
                   !string.IsNullOrWhiteSpace(demandeaccord.Email) &&
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
}