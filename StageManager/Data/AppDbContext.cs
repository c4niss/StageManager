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
        public DbSet<Convention> Conventions { get; set; }
        public DbSet<Demandeaccord> DemandesAccord { get; set; }
        public DbSet<DemandeDepotMemoire> DemandesDepotMemoire { get; set; }
        public DbSet<DemandeDeStage> DemandesDeStage { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Domaine> Domaines { get; set; }
        public DbSet<FicheDePointage> FichesDePointage { get; set; }
        public DbSet<FicheEvaluationEncadreur> FichesEvaluationEncadreur { get; set; }
        public DbSet<FicheEvaluationStagiaire> FichesEvaluationStagiaire { get; set; }
        public DbSet<Memoire> Memoires { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }

        // DbSets pour un accès plus facile aux types spécifiques
        public DbSet<Stagiaire> Stagiaires { get; set; }
        public DbSet<ChefDepartement> ChefDepartements { get; set; }
        public DbSet<Encadreur> Encadreurs { get; set; }
        // Ajoutez MembreDirection si nécessaire
        public DbSet<MembreDirection> MembresDirection { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Utilisateur>()
                .HasDiscriminator<string>("TypeUtilisateur")
                .HasValue<Stagiaire>("Stagiaire")
                .HasValue<Encadreur>("Encadreur")
                .HasValue<ChefDepartement>("ChefDepartement")
                .HasValue<MembreDirection>("MembreDirection");

            // Vous pouvez aussi configurer l'index sur le discriminateur pour améliorer les performances
            modelBuilder.Entity<Utilisateur>()
                .HasIndex("TypeUtilisateur");
            // Relation optionnelle si vous gardez DeleteBehavior.ClientSetNull
            modelBuilder.Entity<DemandeDeStage>()
                .HasOne(d => d.MembreDirection)
                .WithMany()
                .HasForeignKey(d => d.MembreDirectionId)
                .HasPrincipalKey(u => u.Id) // Référence la table Utilisateurs
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Encadreur>()
                .HasOne(e => e.Avenant)
                .WithOne(a => a.Encadreur)
                .HasForeignKey<Avenant>(a => a.EncadreurId)
                .OnDelete(DeleteBehavior.ClientNoAction);
            // Relation Convention-Stage
            modelBuilder.Entity<Stage>()
                .HasOne(s => s.Convention)
                .WithOne(c => c.Stage)
                .HasForeignKey<Convention>(c => c.StageId);
            modelBuilder.Entity<Convention>()
                .HasOne(c => c.MembreDirection)
                .WithMany(m => m.Conventions) // MembreDirection a une liste de Conventions
                .HasForeignKey(c => c.MembreDirectionId)
                .HasPrincipalKey(u => u.Id) // Clé dans Utilisateurs
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Stage>()
                .HasOne(s => s.Encadreur)
                .WithMany(e => e.Stages)
                .HasForeignKey(s => s.EncadreurId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FicheDePointage>()
                .HasOne(f => f.Encadreur)
                .WithMany(e => e.FichesDePointage) // Si Encadreur a une navigation inverse
                .HasForeignKey(f => f.EncadreurId)
                .HasPrincipalKey(u => u.Id) // Référence Utilisateurs.Id
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<FicheDePointage>()
                .HasOne(f => f.Stagiaire)
                .WithOne(s => s.FicheDePointage)
                .HasForeignKey<FicheDePointage>(f => f.StagiaireId)
                .HasPrincipalKey<Stagiaire>(s => s.Id) // Référence Utilisateurs.Id
                .OnDelete(DeleteBehavior.ClientNoAction);
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
            modelBuilder.Entity<Stagiaire>()
                .HasOne(s => s.Demandeaccord)
                .WithMany(d => d.stagiaires)
                .HasForeignKey(s => s.DemandeaccordId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Stagiaire>()
                .HasOne(s => s.Attestation)
                .WithOne(a => a.Stagiaire)
                .HasForeignKey<Attestation>(a => a.StagiaireId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<FicheEvaluationEncadreur>()
                .HasOne(f => f.Encadreur)
                .WithMany(e => e.FichesEvaluationEncadreur) // Encadreur a une collection
                .HasForeignKey(f => f.EncadreurId)
                .HasPrincipalKey(u => u.Id) // Référence Utilisateurs.Id
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<FicheEvaluationStagiaire>()
                .HasOne(f => f.Encadreur)
                .WithMany(e => e.FichesEvaluationStagiaire) // Si Encadreur a une navigation inverse
                .HasForeignKey(f => f.EncadreurId)
                .HasPrincipalKey(u => u.Id) // Référence Utilisateurs.Id
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<FicheEvaluationStagiaire>()
                .HasOne(f => f.Stagiaire)
                .WithOne(s => s.FicheEvaluationStagiaire)
                .HasForeignKey<FicheEvaluationStagiaire>(f => f.StagiaireId)
                .HasPrincipalKey<Stagiaire>(s => s.Id) // Référence Utilisateurs.Id
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DemandeDepotMemoire>()
                .HasOne(d => d.Encadreur)
                .WithMany()
                .HasForeignKey(d => d.EncadreurId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Demandeaccord>()
                .HasOne(d => d.ChefDepartement)
                .WithMany()
                .HasForeignKey(d => d.ChefDepartementId)
                .OnDelete(DeleteBehavior.SetNull);

        }

    }

}