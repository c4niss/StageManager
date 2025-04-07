using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.StagiaireDTO;
using StageManager.Mapping;
using StageManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagiairesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public StagiairesController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// Récupère la liste de tous les stagiaires
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> GetStagiaires()
        {
            var stagiaires = await _db.Stagiaires.ToListAsync();
            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        /// <summary>
        /// Récupère les informations d'un stagiaire spécifique
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StagiaireDto>> GetStagiaire(int id)
        {
            var stagiaire = await _db.Stagiaires
                .Include(s => s.DemandeDeStage)
                .Include(s => s.Stage)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            return Ok(stagiaire.ToDto());
        }

        /// <summary>
        /// Crée un nouveau stagiaire
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StagiaireDto>> CreateStagiaire([FromBody] StagiaireCreateDto stagiaireDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Vérifier si l'email existe déjà
            if (await _db.Stagiaires.AnyAsync(s => s.Email == stagiaireDto.Email))
            {
                return BadRequest($"Un stagiaire avec l'email {stagiaireDto.Email} existe déjà.");
            }

            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Stagiaire>();

            var stagiaire = new Stagiaire
            {
                Nom = stagiaireDto.Nom,
                Prenom = stagiaireDto.Prenom,
                Email = stagiaireDto.Email,
                Telephone = stagiaireDto.Telephone,
                MotDePasse = passwordHasher.HashPassword(null, stagiaireDto.MotDePasse),
                Role = "Stagiaire",
                Universite = stagiaireDto.Universite,
                EstActif = true,
                Specialite = stagiaireDto.Specialite,
                PhotoUrl = stagiaireDto.PhotoUrl,
                Status = StagiaireStatus.EnCour,
                DateCreation = DateTime.Now
            };

            _db.Stagiaires.Add(stagiaire);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStagiaire), new { id = stagiaire.Id }, stagiaire.ToDto());
        }

        /// <summary>
        /// Met à jour les informations d'un stagiaire
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StagiaireDto>> UpdateStagiaire([FromRoute] int id, [FromBody] StagiaireUpdateDto stagiaireDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            // Vérifier si l'email mis à jour existe déjà pour un autre utilisateur
            if (!string.IsNullOrEmpty(stagiaireDto.Email) &&
                stagiaireDto.Email != stagiaire.Email &&
                await _db.Stagiaires.AnyAsync(s => s.Email == stagiaireDto.Email))
            {
                return BadRequest($"Un stagiaire avec l'email {stagiaireDto.Email} existe déjà.");
            }

            if (!string.IsNullOrEmpty(stagiaireDto.Nom))
                stagiaire.Nom = stagiaireDto.Nom;

            if (!string.IsNullOrEmpty(stagiaireDto.Prenom))
                stagiaire.Prenom = stagiaireDto.Prenom;

            if (!string.IsNullOrEmpty(stagiaireDto.Email))
                stagiaire.Email = stagiaireDto.Email;

            if (!string.IsNullOrEmpty(stagiaireDto.Telephone))
                stagiaire.Telephone = stagiaireDto.Telephone;

            if (!string.IsNullOrEmpty(stagiaireDto.Universite))
                stagiaire.Universite = stagiaireDto.Universite;

            if (!string.IsNullOrEmpty(stagiaireDto.Specialite))
                stagiaire.Specialite = stagiaireDto.Specialite;

            if (!string.IsNullOrEmpty(stagiaireDto.PhotoUrl))
                stagiaire.PhotoUrl = stagiaireDto.PhotoUrl;

            stagiaire.Status = stagiaireDto.Status;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(stagiaire.ToDto());
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Une erreur s'est produite lors de la mise à jour du stagiaire: {ex.Message}");
            }
        }

        /// <summary>
        /// Met à jour uniquement le statut d'un stagiaire
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StagiaireDto>> UpdateStagiaireStatus([FromRoute] int id, [FromBody] UpdateStagiaireStatusDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            stagiaire.Status = updateDto.Status;

            await _db.SaveChangesAsync();
            return Ok(stagiaire.ToDto());
        }

        /// <summary>
        /// Met à jour partiellement un stagiaire à l'aide de JSON Patch
        /// </summary>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StagiaireDto>> UpdateStagiairePartiel([FromRoute] int id, [FromBody] JsonPatchDocument<Stagiaire> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Le document patch est null.");
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            patchDoc.ApplyTo(stagiaire, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.SaveChangesAsync();
                return Ok(stagiaire.ToDto());
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Une erreur s'est produite lors de la mise à jour partielle du stagiaire: {ex.Message}");
            }
        }

        /// <summary>
        /// Supprime un stagiaire
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteStagiaire([FromRoute] int id)
        {
            var stagiaire = await _db.Stagiaires
                .Include(s => s.DemandeDeStage)
                .Include(s => s.Stage)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stagiaire == null)
            {
                return NotFound($"Stagiaire avec l'ID {id} non trouvé.");
            }

            // Vérifier si le stagiaire est associé à un stage actif
            if (stagiaire.Stage != null &&
                (stagiaire.Stage.Statut == StatutStage.EnAttente ||
                 stagiaire.Stage.Statut == StatutStage.EnCours ||
                 stagiaire.Stage.Statut == StatutStage.Prolonge))
            {
                return BadRequest("Impossible de supprimer un stagiaire avec un stage actif.");
            }

            try
            {
                _db.Stagiaires.Remove(stagiaire);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Une erreur s'est produite lors de la suppression du stagiaire: {ex.Message}");
            }
        }

        /// <summary>
        /// Recherche des stagiaires selon des critères spécifiques
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> SearchStagiaires(
            [FromQuery] string nom = null,
            [FromQuery] string prenom = null,
            [FromQuery] string universite = null,
            [FromQuery] string specialite = null,
            [FromQuery] StagiaireStatus? status = null)
        {
            var query = _db.Stagiaires.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nom))
                query = query.Where(s => s.Nom.Contains(nom));

            if (!string.IsNullOrWhiteSpace(prenom))
                query = query.Where(s => s.Prenom.Contains(prenom));

            if (!string.IsNullOrWhiteSpace(universite))
                query = query.Where(s => s.Universite.Contains(universite));

            if (!string.IsNullOrWhiteSpace(specialite))
                query = query.Where(s => s.Specialite.Contains(specialite));

            if (status.HasValue)
                query = query.Where(s => s.Status == status.Value);

            var stagiaires = await query.ToListAsync();
            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        /// <summary>
        /// Récupère les stagiaires par statut
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> GetStagiairesByStatus(StagiaireStatus status)
        {
            var stagiaires = await _db.Stagiaires
                .Where(s => s.Status == status)
                .ToListAsync();

            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        /// <summary>
        /// Récupère les stagiaires affectés à un encadreur spécifique
        /// </summary>
        [HttpGet("encadreur/{encadreurId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<StagiaireDto>>> GetStagiairesByEncadreur(int encadreurId)
        {
            // Vérifier que l'encadreur existe
            if (!await _db.Encadreurs.AnyAsync(e => e.Id == encadreurId))
            {
                return NotFound($"Encadreur avec l'ID {encadreurId} non trouvé.");
            }

            var stagiaires = await _db.Stagiaires
                .Where(s => s.Stage != null && s.Stage.EncadreurId == encadreurId)
                .ToListAsync();

            return Ok(stagiaires.Select(s => s.ToDto()));
        }

        /// <summary>
        /// Point d'accès protégé nécessitant une authentification
        /// </summary>
        [Authorize]
        [HttpGet("authenticated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AuthenticatedEndpoint()
        {
            return Ok("Vous êtes authentifié !");
        }
    }
}