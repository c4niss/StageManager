public class MemoireReadDto
{
    public int Id { get; set; }
    public string Titre { get; set; }
    public string CheminFichier { get; set; }
    public DateTime DateDepot { get; set; }
    public int DemandeDepotMemoireId { get; set; }
    public int StageId { get; set; }

    // Informations supplémentaires pour l'affichage
    public string NomPrenomStagiaire { get; set; }
    public string ThemeStage { get; set; }
}
