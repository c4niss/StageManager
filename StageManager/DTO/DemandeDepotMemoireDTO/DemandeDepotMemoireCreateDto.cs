using System.ComponentModel.DataAnnotations;

public class DemandeDepotMemoireCreateDto
{
    [Required(ErrorMessage = "L'identifiant du stage est obligatoire")]
    public int StageId { get; set; }

    [Required(ErrorMessage = "L'identifiant de l'encadreur est obligatoire")]
    public int EncadreurId { get; set; }
}
