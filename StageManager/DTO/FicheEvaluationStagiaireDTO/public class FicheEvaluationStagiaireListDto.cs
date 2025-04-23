public class FicheEvaluationStagiaireListDto
{
    public int Id { get; set; }
    public string NomPrenomStagiaire { get; set; }
    public string FormationStagiaire { get; set; }
    public string ThemeStage { get; set; }
    public string NomPrenomEncadreur { get; set; }
    public DateTime DateEvaluation { get; set; }
    public double ScoreMoyen { get; set; }
}
