
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DemandeaccordDTO;
using StageManager.Models;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeaccordsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DemandeaccordsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Demandeaccord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeaccordDto>>> GetDemandesAccord()
        {
            var demandes = await _context.DemandesAccord
                .Include(d => d.Stagiaire)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .ToListAsync();

            return demandes.Select(d => new DemandeaccordDto
            {
                Id = d.Id,
                FichePieceJointe = d.FichePieceJointe,
                Status = d.Status,
                StagiaireId = d.StagiareId,
                StagiaireNomComplet = $"{d.Stagiaire?.Nom} {d.Stagiaire?.Prenom}",
                ThemeId = d.ThemeId,
                ThemeNom = d.Theme?.Nom,
                EncadreurId = d.EncadreurId,
                EncadreurNomComplet = d.Encadreur != null ? $"{d.Encadreur.Nom} {d.Encadreur.Prenom}" : null
            }).ToList();
        }

        // GET: api/Demandeaccord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DemandeaccordDto>> GetDemandeaccord(int id)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.Stagiaire)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demandeaccord == null)
            {
                return NotFound();
            }

            return new DemandeaccordDto
            {
                Id = demandeaccord.Id,
                FichePieceJointe = demandeaccord.FichePieceJointe,
                Status = demandeaccord.Status,
                StagiaireId = demandeaccord.StagiareId,
                StagiaireNomComplet = $"{demandeaccord.Stagiaire?.Nom} {demandeaccord.Stagiaire?.Prenom}",
                ThemeId = demandeaccord.ThemeId,
                ThemeNom = demandeaccord.Theme?.Nom,
                EncadreurId = demandeaccord.EncadreurId,
                EncadreurNomComplet = demandeaccord.Encadreur != null ? $"{demandeaccord.Encadreur.Nom} {demandeaccord.Encadreur.Prenom}" : null
            };
        }

        // POST: api/Demandeaccord
        [HttpPost]
        public async Task<ActionResult<DemandeaccordDto>> CreateDemandeaccord([FromForm] CreateDemandeaccordDto dto)
        {
            // Vérifications des entités
            var stagiaire = await _context.Stagiaires.FindAsync(dto.StagiaireId);
            if (stagiaire == null)
            {
                return BadRequest("Stagiaire non trouvé");
            }

            var theme = await _context.Themes.FindAsync(dto.ThemeId);
            if (theme == null)
            {
                return BadRequest("Thème non trouvé");
            }

            if (dto.EncadreurId.HasValue)
            {
                var encadreur = await _context.Encadreurs.FindAsync(dto.EncadreurId);
                if (encadreur == null)
                {
                    return BadRequest("Encadreur non trouvé");
                }
            }

            // Gérer le fichier joint
            string uniqueFileName = null;
            if (dto.FichePieceJointe != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "demandes");
                Directory.CreateDirectory(uploadsFolder); // Assurez-vous que le dossier existe

                uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.FichePieceJointe.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.FichePieceJointe.CopyToAsync(fileStream);
                }
            }

            // Créer la demande d'accord
            var demandeaccord = new Demandeaccord
            {
                FichePieceJointe = uniqueFileName,
                Status = StatusAccord.EnAttente,
                StagiareId = dto.StagiaireId,
                ThemeId = dto.ThemeId,
                EncadreurId = dto.EncadreurId,
                // Remarque : DemandeStageId doit être défini, mais il n'est pas dans votre DTO
                // Vous devrez peut-être l'ajouter ou le gérer différemment
                DemandeStageId = 0 // À remplacer par une valeur appropriée
            };

            _context.DemandesAccord.Add(demandeaccord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDemandeaccord),
                new { id = demandeaccord.Id },
                new DemandeaccordDto
                {
                    Id = demandeaccord.Id,
                    FichePieceJointe = demandeaccord.FichePieceJointe,
                    Status = demandeaccord.Status,
                    StagiaireId = demandeaccord.StagiareId,
                    StagiaireNomComplet = $"{stagiaire.Nom} {stagiaire.Prenom}",
                    ThemeId = demandeaccord.ThemeId,
                    ThemeNom = theme.Nom,
                    EncadreurId = demandeaccord.EncadreurId
                });
        }

        // PUT: api/Demandeaccord/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateDemandeaccordStatus(int id, UpdateDemandeaccordStatusDto dto)
        {
            var demandeaccord = await _context.DemandesAccord.FindAsync(id);
            if (demandeaccord == null)
            {
                return NotFound();
            }

            demandeaccord.Status = dto.Status;
            // Gérer le commentaire si nécessaire (pas inclus dans votre modèle actuel)

            _context.DemandesAccord.Update(demandeaccord);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}