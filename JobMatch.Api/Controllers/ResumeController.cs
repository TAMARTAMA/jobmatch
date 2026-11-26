using JobMatch.Api.Data;
using JobMatch.Api.Dtos;
using JobMatch.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobMatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumesController(JobMatchDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<List<ResumeDto>> GetAll() =>
        (await db.Resumes
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync())
        .Select(ToDto)
        .ToList();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResumeDto>> GetById(int id)
    {
        var resume = await db.Resumes.FindAsync(id);
        return resume is null ? NotFound() : ToDto(resume);
    }

    [HttpPost]
    public async Task<ActionResult<ResumeDto>> Create(SaveResumeDto dto)
    {
        var resume = new Resume { Title = dto.Title, Content = dto.Content };
        db.Resumes.Add(resume);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = resume.Id }, ToDto(resume));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SaveResumeDto dto)
    {
        var resume = await db.Resumes.FindAsync(id);
        if (resume is null) return NotFound();

        resume.Title = dto.Title;
        resume.Content = dto.Content;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resume = await db.Resumes.FindAsync(id);
        if (resume is null) return NotFound();

        db.Resumes.Remove(resume);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static ResumeDto ToDto(Resume r) => new(r.Id, r.Title, r.Content, r.CreatedAt);
}