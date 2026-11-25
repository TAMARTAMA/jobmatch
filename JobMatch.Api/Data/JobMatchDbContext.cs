using JobMatch.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatch.Api.Data;

public class JobMatchDbContext(DbContextOptions<JobMatchDbContext> options) : DbContext(options)
{
    public DbSet<JobPosting> JobPostings => Set<JobPosting>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<JobPostingSkill> JobPostingSkills => Set<JobPostingSkill>();
    public DbSet<MatchResult> MatchResults => Set<MatchResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobPosting>(e =>
        {
            e.Property(j => j.Title).HasMaxLength(200);
            e.Property(j => j.Company).HasMaxLength(200);
            e.Property(j => j.Location).HasMaxLength(100);
            e.Property(j => j.Url).HasMaxLength(500);
            e.Property(j => j.Status).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(j => j.Status);
        });

        modelBuilder.Entity<Resume>(e => e.Property(r => r.Title).HasMaxLength(200));

        modelBuilder.Entity<Skill>(e =>
        {
            e.Property(s => s.Name).HasMaxLength(100);
            e.HasIndex(s => s.Name).IsUnique();
        });

        modelBuilder.Entity<JobPostingSkill>(e =>
            e.HasKey(js => new { js.JobPostingId, js.SkillId }));

        modelBuilder.Entity<MatchResult>(e =>
            e.Property(m => m.Score).HasPrecision(5, 2));
    }
}