namespace JobMatch.Api.Agents;

public class JobAnalysis
{
    public List<string> RequiredSkills { get; set; } = [];
    public List<string> NiceToHaveSkills { get; set; } = [];
    public int? MinYearsExperience { get; set; }
    public string? Seniority { get; set; }
    public string Summary { get; set; } = "";
}