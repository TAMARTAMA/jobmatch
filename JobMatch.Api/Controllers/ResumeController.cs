// using JobMatch.Api.Data;
// using JobMatch.Api.Dtos;
// using JobMatch.Api.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace JobMatch.Api.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class ResumeController(JobMatchDbContext db) : ControllerBase
// {
//     [HttpGet]
//     public async Task<List<ResumeDto>> GetAll() =>
//         (await db.Resumes
//             .OrderByDescending(r => r.CreatedAt)
//             .ToListAsync())
//         .Select(ToDto)
//         .ToList();
//     [HttpGet("{id:int}")]
//     public async Task<ActionResult<ResumeDto>> GetById(int id)
//     {
//         var resume = await db.Resumes.FindAsync(id);
//         return resume is null ? NotFound() : ToDto(resume);
//     }
//     [HttpPost]
//     public async Task<ActionResult<ResumeDto>> Create(CreateResumeDto dto)
//     {
//         var resume = new Resume { Name = dto.Name, Email = dto.Email, Phone = dto.Phone, Address = dto.Address, City = dto.City, State = dto.State, Zip = dto.Zip, Country = dto.Country, Summary = dto.Summary, Skills = dto.Skills, Experience = dto.Experience, Education = dto.Education };
//     }
//     [HttpPut("{id:int}")]
//     public async Task<ActionResult<ResumeDto>> Update(int id, UpdateResumeDto dto)
//     {
//         var resume = await db.Resumes.FindAsync(id);
//         if (resume is null) return NotFound();
//         resume.Name = dto.Name;
//         resume.Email = dto.Email;
//         resume.Phone = dto.Phone;
//         resume.Address = dto.Address;
//         resume.City = dto.City;
//         resume.State = dto.State;
//         resume.Zip = dto.Zip;
//     }
// }