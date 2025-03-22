using Microsoft.EntityFrameworkCore;
using StageManager.Models;

namespace TestRestApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Stagiaire> Stagiaires { get; set; }
        public DbSet<DemandeDeStage> DemandesDeStage { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation one-to-many entre DemandeDeStage et Stagiaire
            modelBuilder.Entity<DemandeDeStage>()
                .HasMany(d => d.Stagiaires)
                .WithOne(s => s.DemandeDeStage)
                .HasForeignKey(s => s.DemandeDeStageId);

            // Contrainte unique pour garantir qu'un stagiaire ne peut avoir qu'une seule demande de stage
            modelBuilder.Entity<Stagiaire>()
                .HasIndex(s => s.DemandeDeStageId)
                .IsUnique();
        }
    }
}
