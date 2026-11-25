namespace JobMatch.Api.Models;
public class JobPostingSkill
{
    public int JobPostingId { get; set; }
    public JobPosting JobPosting { get; set; } = null!;
    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public bool IsRequired { get; set; }
}