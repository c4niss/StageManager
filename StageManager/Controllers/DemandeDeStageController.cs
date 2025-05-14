using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using StageManager.DTO.DemandeDeStageDTO;
using StageManager.Mapping;
using StageManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;
using System.Net.Mail;
using MailKit.Net.Smtp;
using System.Security.Claims;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeDeStageController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public DemandeDeStageController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpGet]
        [Authorize(Roles = "MembreDirection")]
        public async Task<ActionResult<IEnumerable<DemandeDeStageDto>>> GetDemandesDeStage()
        {
            var demandes = await _db.DemandesDeStage.Include(d => d.Stagiaires).Include(d => d.MembreDirection).ToListAsync();
            return Ok(demandes.Select(d => d.ToDto()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "MembreDirection")]
        public async Task<ActionResult<DemandeDeStageDto>> GetDemandeDeStage(int id)
        {
            var demande = await _db.DemandesDeStage.Include(d => d.Stagiaires).FirstOrDefaultAsync(d => d.Id == id);
            if (demande == null) return NotFound("Demande de stage non trouvée.");
            return Ok(demande.ToDto());
        }

        [HttpPost]
        [Authorize(Roles = "MembreDirection")]
        public async Task<ActionResult<DemandeDeStageDto>> CreateDemandeDeStage([FromBody] DemandeDeStageCreateDto demandeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stagiaires = await _db.Stagiaires.Where(s => demandeDto.StagiaireIds.Contains(s.Id)).ToListAsync();
            if (stagiaires.Count != demandeDto.StagiaireIds.Count) return BadRequest("Un ou plusieurs stagiaires n'existent pas.");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int membreDirectionId)) return BadRequest("Impossible d'identifier le membre de direction.");
            var membreDirection = await _db.MembresDirection.FindAsync(membreDirectionId);
            if (membreDirection == null) return BadRequest("Le membre de direction spécifié n'existe pas.");
            if (!membreDirection.EstActif) return BadRequest("Votre compte est inactif. Veuillez contacter l'administrateur.");
            var demande = new DemandeDeStage
            {
                DateDemande = DateTime.Now,
                cheminfichier = demandeDto.CheminFichier,
                Statut = DemandeDeStage.StatusDemandeDeStage.Enattente,
                Stagiaires = stagiaires,
                MembreDirectionId = membreDirectionId
            };
            _db.DemandesDeStage.Add(demande);
            await _db.SaveChangesAsync();
            foreach (var stagiaire in stagiaires) stagiaire.DemandeDeStageId = demande.Id;
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDemandeDeStage), new { id = demande.Id }, demande.ToDto());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "MembreDirection")]
        public async Task<ActionResult<DemandeDeStageDto>> UpdateDemandeDeStage(int id, [FromBody] DemandeDeStageUpdateDto demandeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var demande = await _db.DemandesDeStage.Include(d => d.Stagiaires).Include(d => d.MembreDirection).FirstOrDefaultAsync(d => d.Id == id);
            if (demande == null) return NotFound("Demande de stage non trouvée.");
            if (!string.IsNullOrEmpty(demandeDto.CheminFichier)) demande.cheminfichier = demandeDto.CheminFichier;
            demande.Statut = demandeDto.Statut;
            demande.Commentaire = demandeDto.Commentaire;
            if (demande.Statut == DemandeDeStage.StatusDemandeDeStage.Accepte)
            {
                var existingAccord = await _db.DemandesAccord.Include(a => a.stagiaires).FirstOrDefaultAsync(a => a.DemandeStageId == demande.Id);
                Demandeaccord demandeAccord;
                if (existingAccord == null)
                {
                    demandeAccord = new Demandeaccord
                    {
                        DemandeStageId = demande.Id,
                        DateCreation = DateTime.Now,
                        stagiaires = new List<Stagiaire>(),
                    };
                    _db.DemandesAccord.Add(demandeAccord);
                    await _db.SaveChangesAsync();
                    demande.DemandeaccordId = demandeAccord.Id;
                }
                else demandeAccord = existingAccord;
                foreach (var stagiaire in demande.Stagiaires)
                {
                    if (!demandeAccord.stagiaires.Any(s => s.Id == stagiaire.Id)) demandeAccord.stagiaires.Add(stagiaire);
                    stagiaire.Status = StagiaireStatus.Accepte;
                    await EnvoyerEmailAsync(stagiaire.Email, "Demande de stage acceptée", $"Bonjour {stagiaire.Prenom},\n\nVotre demande de stage n°{demande.Id} a été acceptée.\nVous pouvez maintenant compléter votre dossier d'accord en cliquant sur le lien suivant :\n{GetAccordFormLink(demandeAccord.Id)}\n\nCordialement,\nLe service des stages");
                }
                var stage = new Stage
                {
                    StagiaireGroup = string.Join(", ", demande.Stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
                    Statut = StatutStage.EnAttente,
                    Stagiaires = new List<Stagiaire>()
                };
                foreach (var stagiaire in demandeAccord.stagiaires)
                {
                    stage.Stagiaires.Add(stagiaire);
                    stagiaire.StageId = stage.Id;
                }
                _db.Stages.Add(stage);
                await _db.SaveChangesAsync();
            }
            else if (demande.Statut == DemandeDeStage.StatusDemandeDeStage.Refuse)
            {
                foreach (var stagiaire in demande.Stagiaires)
                {
                    stagiaire.Status = StagiaireStatus.Refuse;
                    await EnvoyerEmailAsync(stagiaire.Email, "Demande de stage refusée", $"Bonjour {stagiaire.Prenom},\n\nNous regrettons de vous informer que votre demande de stage n'a pas été retenue.\n{demandeDto.Commentaire}\n\nCordialement,\nLe service des stages");
                }
            }
            await _db.SaveChangesAsync();
            return Ok(demande.ToDto());
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

        private string GetAccordFormLink(int accordId)
        {
            return $"{_config["Frontend:BaseUrl"]}/accord/{accordId}/completer";
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MembreDirection")]
        public async Task<ActionResult> DeleteDemandeDeStage(int id)
        {
            var demande = await _db.DemandesDeStage.Include(d => d.Stagiaires).FirstOrDefaultAsync(d => d.Id == id);
            if (demande == null) return NotFound("Demande de stage non trouvée.");
            foreach (var stagiaire in demande.Stagiaires) stagiaire.DemandeDeStageId = null;
            _db.DemandesDeStage.Remove(demande);
            await _db.SaveChangesAsync();
            return NoContent();
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
    }
}