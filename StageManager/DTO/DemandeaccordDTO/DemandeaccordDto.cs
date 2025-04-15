using StageManager.Models;
using System.ComponentModel.DataAnnotations;

public class DemandeaccordDto
{
    public int Id { get; set; }
    [EnumDataType(typeof(StatusAccord))]
    public StatusAccord Status { get; set; }
    public List<int> StagiaireId { get; set; }
    public string? StagiaireNomComplet { get; set; }
    public int? ThemeId { get; set; }
    public string? ThemeNom { get; set; }
    public int DemandeStageId { get; set; }
    public int? EncadreurId { get; set; }
    public string? EncadreurNomComplet { get; set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public int? NombreSeancesParSemaine { get; set; }
    public int? DureeSeances { get; set; }
    public string? ServiceAccueil { get; set; }
    public string? UniversiteInstitutEcole { get; set; }
    public string? FiliereSpecialite { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? DiplomeObtention { get; set; }
    public NatureStage? NatureStage { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public DateTime DateCreation { get; set; }
}