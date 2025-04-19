using StageManager.DTO.ChefDepartementDTO;
using StageManager.Models;
using System.Security.Cryptography;
using System.Text;

namespace StageManager.Mapping
{
    public static class ChefDepartementMapping
    {
        public static ChefDepartementDto ToDto(this ChefDepartement chef)
        {
            return new ChefDepartementDto
            {
                Id = chef.Id,
                Nom = chef.Nom,
                Prenom = chef.Prenom,
                Email = chef.Email,
                Telephone = chef.Telephone ?? string.Empty,
                EstActif = chef.EstActif,
                PhotoUrl = chef.PhotoUrl ?? string.Empty,
                DepartementId = chef.DepartementId,
                DepartementNom = chef.Departement?.Nom ?? string.Empty,
            };
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}