using StageManager.Models;

public class StageMinimalDto
{
    public int Id { get; set; }
    public Theme Theme { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public string TypeStage { get; set; }
}