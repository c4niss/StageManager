﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO.DomaineDTO;
using StageManager.Mapping;
using StageManager.Models;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DomainesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Domaines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DomaineDto>>> GetDomaines()
        {
            var domaines = await _context.Domaines
                .Include(d => d.Departement)
                .Include(d => d.Encadreurs)
                .Include(d => d.Stages)
                .ToListAsync();

            return Ok(domaines.Select(d => d.ToDto()));
        }

        // GET: api/Domaines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DomaineDto>> GetDomaine(int id)
        {
            var domaine = await _context.Domaines
                .Include(d => d.Departement)
                .Include(d => d.Encadreurs)
                .Include(d => d.Stages)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (domaine == null)
            {
                return NotFound();
            }

            return Ok(domaine.ToDto());
        }

        // POST: api/Domaines
        [HttpPost]
        public async Task<ActionResult<DomaineDto>> CreateDomaine(CreateDomaineDto createDto)
        {
            var departement = await _context.Departements.FindAsync(createDto.DepartementId);
            if (departement == null)
            {
                return BadRequest("Département non trouvé");
            }

            // Vérifier si le nom du domaine existe déjà dans le même département
            var existingDomaine = await _context.Domaines
                .AnyAsync(d => d.Nom == createDto.Nom && d.DepartementId == createDto.DepartementId);
            if (existingDomaine)
            {
                return BadRequest("Un domaine avec ce nom existe déjà dans ce département");
            }

            var domaine = new Domaine
            {
                Nom = createDto.Nom,
                DepartementId = createDto.DepartementId
            };

            _context.Domaines.Add(domaine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDomaine), new { id = domaine.Id }, domaine.ToDto());
        }

        // PUT: api/Domaines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDomaine(int id, UpdateDomaineDto updateDto)
        {
            var domaine = await _context.Domaines.FindAsync(id);
            if (domaine == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateDto.Nom))
                domaine.Nom = updateDto.Nom;

            if (updateDto.DepartementId.HasValue)
            {
                var departement = await _context.Departements.FindAsync(updateDto.DepartementId.Value);
                if (departement == null)
                {
                    return BadRequest("Département non trouvé");
                }
                domaine.DepartementId = updateDto.DepartementId.Value;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomaineExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }
        // DELETE: api/Domaines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDomaine(int id)
        {
            var domaine = await _context.Domaines.FindAsync(id);
            if (domaine == null)
            {
                return NotFound();
            }

            // Mettre à null le DomaineId pour tous les encadreurs associés
            var encadreurs = await _context.Encadreurs
                .Where(e => e.DomaineId == id)
                .ToListAsync();
            foreach (var encadreur in encadreurs)
            {
                encadreur.DomaineId = null;
            }

            _context.Domaines.Remove(domaine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DomaineExists(int id)
        {
            return _context.Domaines.Any(e => e.Id == id);
        }
    }
}