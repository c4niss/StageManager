using System.ComponentModel.DataAnnotations;

public class MemoireCreateDto
{
    [Required(ErrorMessage = "Le titre du mémoire est obligatoire")]
    [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
    public string Titre { get; set; }

    [Required(ErrorMessage = "Le chemin du fichier est obligatoire")]
    [StringLength(255, ErrorMessage = "Le chemin du fichier ne peut pas dépasser 255 caractères")]
    public string CheminFichier { get; set; }

    [Required(ErrorMessage = "L'identifiant de la demande de dépôt est obligatoire")]
    public int DemandeDepotMemoireId { get; set; }

    [Required(ErrorMessage = "L'identifiant du stage est obligatoire")]
    public int StageId { get; set; }
}
