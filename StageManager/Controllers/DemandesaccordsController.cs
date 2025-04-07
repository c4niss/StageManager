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
                .Include(d => d.stagiaires)  // Assurez-vous que le nom de la propriété est correct (Stagiaires au lieu de stagiaires)
                .Include(d => d.Theme)
                .Include(d => d.Encadreur)
                .AsNoTracking()  // Ajout recommandé pour les opérations en lecture seule
                .ToListAsync();

            if (!demandes.Any())
            {
                return NotFound("Aucune demande d'accord trouvée.");
            }

            var result = demandes.Select(d => new DemandeaccordDto
            {
                Id = d.Id,
                FichePieceJointe = d.FichePieceJointe,
                Status = d.Status,
                StagiaireId = d.stagiaires?.Select(s => s.Id).ToList() ?? new List<int>(),
                StagiaireNomComplet = d.stagiaires != null ?
                    string.Join(", ", d.stagiaires.Select(s => $"{s.Nom} {s.Prenom}")) : string.Empty,
                ThemeId = d.ThemeId,
                ThemeNom = d.Theme?.Nom,
                EncadreurId = d.EncadreurId,
                EncadreurNomComplet = d.Encadreur != null ? $"{d.Encadreur.Nom} {d.Encadreur.Prenom}" : null
            }).ToList();

            return Ok(result);
        }

        // GET: api/Demandeaccord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DemandeaccordDto>> GetDemandeaccord(int id)
        {
            var demandeaccord = await _context.DemandesAccord
                .Include(d => d.stagiaires)
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
                StagiaireId = demandeaccord.stagiaires.Select(s => s.Id).ToList(),
                StagiaireNomComplet = string.Join(", ", demandeaccord.stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
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
            // Vérification des stagiaires
            var stagiaires = new List<Stagiaire>();
            foreach (var stagiaireId in dto.StagiaireId)
            {
                var stagiaire = await _context.Stagiaires.FindAsync(stagiaireId);
                if (stagiaire == null)
                {
                    return BadRequest($"Stagiaire avec l'ID {stagiaireId} non trouvé");
                }
                stagiaires.Add(stagiaire);
            }

            // Vérification du thème
            var theme = await _context.Themes.FindAsync(dto.ThemeId);
            if (theme == null)
            {
                return BadRequest("Thème non trouvé");
            }

            // Vérification de l'encadreur si fourni
            Encadreur encadreur = null;
            if (dto.EncadreurId.HasValue)
            {
                encadreur = await _context.Encadreurs.FindAsync(dto.EncadreurId);
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
                Directory.CreateDirectory(uploadsFolder);

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
                stagiaires = stagiaires,
                ThemeId = dto.ThemeId,
                EncadreurId = dto.EncadreurId,
                DemandeStageId = 0 // À définir selon votre logique
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
                    StagiaireId = demandeaccord.stagiaires.Select(s => s.Id).ToList(),
                    StagiaireNomComplet = string.Join(", ", demandeaccord.stagiaires.Select(s => $"{s.Nom} {s.Prenom}")),
                    ThemeId = demandeaccord.ThemeId,
                    ThemeNom = theme.Nom,
                    EncadreurId = demandeaccord.EncadreurId,
                    EncadreurNomComplet = encadreur != null ? $"{encadreur.Nom} {encadreur.Prenom}" : null
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
            _context.DemandesAccord.Update(demandeaccord);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}