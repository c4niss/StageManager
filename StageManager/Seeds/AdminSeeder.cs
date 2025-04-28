using Microsoft.AspNetCore.Identity;
using StageManager.Models;
using TestRestApi.Data;

namespace StageManager.Seeds
{
    public class AdminSeeder
    {
        public static async Task SeedAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = new PasswordHasher<Utilisateur>();

            // Vérifier si un administrateur existe déjà
            var adminExists = dbContext.Utilisateurs.Any(u => u.Role == UserRoles.Admin);

            if (!adminExists)
            {
                var admin = new Admin
                {
                    Nom = "ATM",
                    Prenom = "Mobilis",
                    Username = "mobi-stage",
                    Email = "atmmobilis@gmail.com",
                    Telephone = "123456789",
                    Role = UserRoles.Admin,
                    EstActif = true,
                };

                // Hasher le mot de passe
                admin.MotDePasse = passwordHasher.HashPassword(null, "Admin@123");

                // Ajouter l'admin à la base de données
                dbContext.Utilisateurs.Add(admin);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
