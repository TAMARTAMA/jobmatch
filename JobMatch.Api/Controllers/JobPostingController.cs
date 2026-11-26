using JobMatch.Api.Data;
using JobMatch.Api.Dtos;
using JobMatch.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobMatch.Api.Agents;

namespace JobMatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobPostingsController(JobMatchDbContext db, ILogger<JobPostingsController> logger) : ControllerBase{
    [HttpGet]
    public async Task<List<JobPostingDto>> GetAll() =>
        (await db.JobPostings
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync())
        .Select(ToDto)
        .ToList();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobPostingDto>> GetById(int id)
    {
        var job = await db.JobPostings.FindAsync(id);
        return job is null ? NotFound() : ToDto(job);
    }

    [HttpPost]
    public async Task<ActionResult<JobPostingDto>> Create(CreateJobPostingDto dto)
    {
        var job = new JobPosting
        {
            Title = dto.Title,
            Company = dto.Company,
            Location = dto.Location,
            Description = dto.Description,
            Url = dto.Url
        };
        db.JobPostings.Add(job);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = job.Id }, ToDto(job));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateJobStatusDto dto)
    {
        if (!Enum.TryParse<JobStatus>(dto.Status, ignoreCase: true, out var status))
            return BadRequest($"Unknown status: {dto.Status}");

        var job = await db.JobPostings.FindAsync(id);
        if (job is null) return NotFound();

        job.Status = status;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await db.JobPostings.FindAsync(id);
        if (job is null) return NotFound();

        db.JobPostings.Remove(job);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static JobPostingDto ToDto(JobPosting j) =>
        new(j.Id, j.Title, j.Company, j.Location, j.Description, j.Url, j.Status.ToString(), j.CreatedAt);

    [HttpGet("{id:int}/skills")]
    
    public async Task<ActionResult<List<JobSkillDto>>> GetSkills(int id)
    {
        if (!await db.JobPostings.AnyAsync(j => j.Id == id)) return NotFound();

        return await db.JobPostingSkills
            .Where(js => js.JobPostingId == id)
            .OrderByDescending(js => js.IsRequired)
            .ThenBy(js => js.Skill.Name)
            .Select(js => new JobSkillDto(js.Skill.Name, js.IsRequired))
            .ToListAsync();
    }

    [HttpPost("{id:int}/analyze")]
    public async Task<ActionResult<JobAnalysis>> Analyze(int id, [FromServices] JobAnalyzerAgent analyzer)
    {
        var job = await db.JobPostings.FindAsync(id);
        if (job is null) return NotFound();

        JobAnalysis analysis;
        try
        {
            analysis = await analyzer.AnalyzeAsync(job.Title, job.Description);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Job analysis failed for job {JobId}", id);
            return Problem(title: "The AI analysis failed", detail: ex.Message, statusCode: 502);
        }

        // מוחקים את הכישורים מניתוח קודם
        await db.JobPostingSkills.Where(js => js.JobPostingId == id).ExecuteDeleteAsync();

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var allSkills = analysis.RequiredSkills.Select(name => (name, isRequired: true))
            .Concat(analysis.NiceToHaveSkills.Select(name => (name, isRequired: false)));

        foreach (var (name, isRequired) in allSkills)
        {
            var clean = name.Trim();
            if (clean.Length is 0 or > 100 || !seen.Add(clean)) continue;

            var skill = await db.Skills.FirstOrDefaultAsync(s => s.Name == clean)
                ?? new Skill { Name = clean };

            db.JobPostingSkills.Add(new JobPostingSkill
            {
                JobPostingId = id,
                Skill = skill,
                IsRequired = isRequired
            });
        }

        await db.SaveChangesAsync();
        return analysis;
    }   
}