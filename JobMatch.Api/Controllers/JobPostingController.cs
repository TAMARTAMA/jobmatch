using JobMatch.Api.Data;
using JobMatch.Api.Dtos;
using JobMatch.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobMatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobPostingsController(JobMatchDbContext db) : ControllerBase
{
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
}