using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StageManager.DTO.StagiaireDTO;
using StageManager.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestRestApi.Data;
using StageManager.Mapping;

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

        [HttpGet]
        public async Task<IActionResult> GetStagiaires()
        {
            var stagiaires = await _db.Stagiaires.ToListAsync();
            return Ok(stagiaires);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStagiaire(int id)
        {
            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound();
            }
            return Ok(stagiaire);
        }

        [HttpPost]
        public async Task<IActionResult> AddStagiaire([FromBody] StagiaireCreateDto stagiairedto)
        {
            var passwordHasher = new PasswordHasher<Stagiaire>();

            var stagiaire = new Stagiaire
            {
                Nom = stagiairedto.Nom,
                Prenom = stagiairedto.Prenom,
                Email = stagiairedto.Email,
                Telephone = stagiairedto.Telephone,
                MotDePasse = passwordHasher.HashPassword(null, stagiairedto.MotDePasse),
                Role = "Stagiaire",
                Universite = stagiairedto.Universite,
                EstActif = true,
                Specialite = stagiairedto.Specialite,
                PhotoUrl = stagiairedto.PhotoUrl
            };

            _db.Stagiaires.Add(stagiaire);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStagiaire), new { id = stagiaire.Id }, stagiaire);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] StagiaireloginDto stagiaireDto)
        {
            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Email == stagiaireDto.Email);
            if (stagiaire == null)
            {
                return NotFound("Aucun utilisateur avec cet email.");
            }

            var passwordHasher = new PasswordHasher<Stagiaire>();
            var result = passwordHasher.VerifyHashedPassword(null, stagiaire.MotDePasse, stagiaireDto.MotDePasse);

            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Mot de passe incorrect.");
            }
            var token = CreateToken(stagiaire);

            return Ok(new
            {
                Token = token,
                Stagiaire =stagiaire
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStagiaire([FromRoute]int id, [FromBody] StagiaireUpdateDto stagiaireDto)
        {
            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound();
            }
            stagiaire.Nom = stagiaireDto.Nom;
            stagiaire.Prenom = stagiaireDto.Prenom;
            stagiaire.Email = stagiaireDto.Email;
            stagiaire.Telephone = stagiaireDto.Telephone;
            stagiaire.Universite = stagiaireDto.Universite;
            stagiaire.Specialite = stagiaireDto.Specialite;
            stagiaire.PhotoUrl = stagiaireDto.PhotoUrl;
            stagiaire.Status = stagiaireDto.Status;
            await _db.SaveChangesAsync();
            return Ok(stagiaire);
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStagiaireStatus([FromRoute] int id, [FromBody] UpdateStagiaireStatusDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound("Stagiaire non trouvé.");
            }


            stagiaire.Status = updateDto.Status;
            await _db.SaveChangesAsync();
            return Ok(stagiaire.ToDto());
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStagiairePartiel([FromRoute] int id, [FromBody] JsonPatchDocument<Stagiaire> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is null.");
            }

            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(stagiaire, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.SaveChangesAsync();
            return Ok(stagiaire.ToDto());
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStagiaire([FromRoute] int id)
        {
            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(s => s.Id == id);
            if (stagiaire == null)
            {
                return NotFound();
            }
            _db.Stagiaires.Remove(stagiaire);
            await _db.SaveChangesAsync();
            
            return Ok();
        }


        private string CreateToken(Stagiaire stagiaire)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, stagiaire.Nom),
                new Claim(ClaimTypes.NameIdentifier, stagiaire.Id.ToString()),
                new Claim(ClaimTypes.Role, stagiaire.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Expiration du token
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [Authorize] 
        [HttpGet("/try")]
        public IActionResult ProtectedEndpoint()
        {
            return Ok("You are authenticated!");
        }
    }
}
