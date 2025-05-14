using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.ThemeDTO;
using StageManager.Mapping;
using StageManager.Models;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThemesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Themes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThemeDto>>> GetThemes()
        {
            var themes = await _context.Themes
                .Include(t => t.Stage)
                .ToListAsync();

            return Ok(themes.Select(t => t.ToDto()));
        }

        // GET: api/Themes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThemeDto>> GetTheme(int id)
        {
            var theme = await _context.Themes
                .Include(t => t.Stage)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (theme == null)
            {
                return NotFound();
            }

            return Ok(theme.ToDto());
        }

        // POST: api/Themes
        [HttpPost]
        public async Task<ActionResult<ThemeDto>> CreateTheme(CreateThemeDto createDto)
        {
            var demandeAccord = await _context.DemandesAccord.FindAsync(createDto.DemandeaccordId);
            if (demandeAccord == null)
            {
                return BadRequest("Demande d'accord non trouvée");
            }

            // Vérifier si un StageId est fourni
            if (createDto.StageId.HasValue)
            {
                var stage = await _context.Stages.FindAsync(createDto.StageId.Value);
                if (stage == null)
                {
                    return BadRequest("Stage non trouvé");
                }
            }
            if (string.IsNullOrEmpty(createDto.Nom))
            {
                return BadRequest("Le nom du thème est requis");
            }
            // Vérifier si le nom du thème existe déjà
            var existingTheme = await _context.Themes
                .AnyAsync(t => t.Nom == createDto.Nom && t.DemandeaccordId == createDto.DemandeaccordId);
            if (existingTheme)
            {
                return BadRequest("Un thème avec ce nom existe déjà pour cette demande d'accord");
            }

            var theme = new Theme
            {
                Nom = createDto.Nom,
                DemandeaccordId = createDto.DemandeaccordId,
                StageId = createDto.StageId
            };

            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTheme), new { id = theme.Id }, theme.ToDto());
        }

        // PUT: api/Themes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheme(int id, UpdateThemeDto updateDto)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateDto.Nom))
                theme.Nom = updateDto.Nom;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThemeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Ajouter une nouvelle méthode pour mettre à jour le StageId plus tard
        [HttpPut("{id}/AssignStage/{stageId}")]
        public async Task<IActionResult> AssignStageToTheme(int id, int stageId)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound("Thème non trouvé");
            }

            var stage = await _context.Stages.FindAsync(stageId);
            if (stage == null)
            {
                return NotFound("Stage non trouvé");
            }

            theme.StageId = stageId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThemeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Themes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheme(int id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            _context.Themes.Remove(theme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThemeExists(int id)
        {
            return _context.Themes.Any(e => e.Id == id);
        }
    }
}