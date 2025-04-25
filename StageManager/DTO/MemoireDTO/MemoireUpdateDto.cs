using System.ComponentModel.DataAnnotations;

public class MemoireUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le titre du mémoire est obligatoire")]
    [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
    public string Titre { get; set; }

    [StringLength(255, ErrorMessage = "Le chemin du fichier ne peut pas dépasser 255 caractères")]
    public string CheminFichier { get; set; }
}