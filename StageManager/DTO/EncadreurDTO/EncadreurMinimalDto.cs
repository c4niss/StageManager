using StageManager.Models;

public class EncadreurMinimalDto
{
    public int Id { get; set; }
    public string NomPrenom { get; set; }
    public string Fonction { get; set; }
    public Departement Departement { get; set; }
}