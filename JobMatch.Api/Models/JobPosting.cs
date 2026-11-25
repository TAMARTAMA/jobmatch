// Models/JobPosting.cs
namespace JobMatch.Api.Models;

public enum JobStatus { Saved, Applied, Interview, Rejected, Offer }

public class JobPosting
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public required string Description { get; set; }
    public string? Url { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Saved;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<JobPostingSkill> Skills { get; set; } = [];
}
