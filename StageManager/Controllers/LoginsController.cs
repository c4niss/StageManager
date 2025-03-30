using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StageManager.DTO.LoginDTO;
using StageManager.DTO.StagiaireDTO;
using StageManager.Models;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public LoginsController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var stagiaire = await _db.Stagiaires.FirstOrDefaultAsync(e => e.Email == loginDto.Email);
            if (stagiaire != null)
            {
                return await AuthenticateUser(stagiaire, loginDto.MotDePasse, "Stagiaire");
            }
            var encadreur = await _db.Encadreurs.FirstOrDefaultAsync(e => e.Email == loginDto.Email);
            if (encadreur != null)
            {
                return await AuthenticateUser(encadreur, loginDto.MotDePasse, "Encadreur");
            }
            var chefdepartement = await _db.ChefDepartements.FirstOrDefaultAsync(e => e.Email == loginDto.Email);
            if (chefdepartement != null)
            {
                return await AuthenticateUser(chefdepartement, loginDto.MotDePasse, "Chefdepartement");
            }
            var membredirection = await _db.MembresDirection.FirstOrDefaultAsync(e => e.Email == loginDto.Email);
            if (membredirection != null)
            {
                return await AuthenticateUser(membredirection, loginDto.MotDePasse, "Membredirection");
            }
            return NotFound("Utilisateur Non Trouver");
        }

        private async Task<IActionResult> AuthenticateUser(Utilisateur Utilisateur, string motdepasse, string userType)
        {
            var passwordHasher = new PasswordHasher<Utilisateur>();
            var result = passwordHasher.VerifyHashedPassword(null, Utilisateur.MotDePasse, motdepasse);

            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Mot de passe incorrect.");
            }
            if(!Utilisateur.EstActif)
            {
                return BadRequest("Votre compte est désactivé.");
            }
            var token = CreateToken(Utilisateur);

            return Ok(new {
                Token = token,
                UserType = userType,
                User = Utilisateur
            });
        }

        private string CreateToken(Utilisateur Utilisateur)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Utilisateur.Nom),
                new Claim(ClaimTypes.NameIdentifier, Utilisateur.Id.ToString()),
                new Claim(ClaimTypes.Role, Utilisateur.Role)
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
    }
}
