namespace JobMatch.Api.Models;
public class MatchResult
{
    public int Id { get; set; }
    public int ResumeId { get; set; }
    public Resume Resume { get; set; } = null!;
    public int JobPostingId { get; set; }
    public JobPosting JobPosting { get; set; } = null!;
    public decimal Score { get; set; }
    public string? MissingSkills { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}