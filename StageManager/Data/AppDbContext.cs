using Microsoft.EntityFrameworkCore;
using StageManager.Models;

namespace TestRestApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Attestation> Attestations { get; set; }
        public DbSet<Avenant> Avenants { get; set; }
        public DbSet<ChefDepartement> ChefDepartements { get; set; }
        public DbSet<Convention> Conventions { get; set; }
        public DbSet<Demandeaccord> DemandesAccord { get; set; }
        public DbSet<DemandeDepotMemoire> DemandesDepotMemoire { get; set; }
        public DbSet<DemandeDeStage> DemandesDeStage { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Domaine> Domaines { get; set; }
        public DbSet<Encadreur> Encadreurs { get; set; }
        public DbSet<FicheDePointage> FichesDePointage { get; set; }
        public DbSet<FicheEvaluationEncadreur> FichesEvaluationEncadreur { get; set; }
        public DbSet<FicheEvaluationStagiaire> FichesEvaluationStagiaire { get; set; }
        public DbSet<MembreDirection> MembresDirection { get; set; }
        public DbSet<Memoire> Memoires { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Stagiaire> Stagiaires { get; set; }
        public DbSet<Theme> Themes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Relation optionnelle si vous gardez DeleteBehavior.ClientSetNull
            modelBuilder.Entity<DemandeDeStage>()
                .HasMany(d => d.Stagiaires)
                .WithOne(s => s.DemandeDeStage)
                .HasForeignKey(s => s.DemandeDeStageId)
                .IsRequired(false)  // Rend la relation optionnelle
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Relation Convention-Stage
            modelBuilder.Entity<Stage>()
                .HasOne(s => s.Convention)
                .WithOne(c => c.Stage)
                .HasForeignKey<Convention>(c => c.StageId);
            modelBuilder.Entity<Stage>()
                .HasOne(s => s.Encadreur)
                .WithMany(e => e.Stages)
                .HasForeignKey(s => s.EncadreurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Encadreur>()
                .HasOne(e => e.Departement)
                .WithMany(d => d.Encadreurs)
                .HasForeignKey(e => e.DepartementId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Memoire>()
                .HasOne(m => m.Stage)
                .WithOne(s => s.Memoire)
                .HasForeignKey<Memoire>(m => m.StageId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            // Configuration pour DemandeDepotMemoire (si one-to-one)
            modelBuilder.Entity<Memoire>()
                .HasOne(m => m.DemandeDepotMemoire)
                .WithOne(d => d.Memoire)
                .HasForeignKey<Memoire>(m => m.DemandeDepotMemoireId)
                .OnDelete(DeleteBehavior.ClientNoAction);
            modelBuilder.Entity<Demandeaccord>()
                .HasMany(d => d.stagiaires)
                .WithOne(s => s.Demandeaccord)
                .HasForeignKey(s => s.DemandeaccordId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }

}