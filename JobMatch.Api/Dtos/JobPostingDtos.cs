using System.ComponentModel.DataAnnotations;

namespace JobMatch.Api.Dtos;

public record JobPostingDto(int Id, string Title, string? Company, string? Location,
    string Description, string? Url, string Status, DateTime CreatedAt);

public record CreateJobPostingDto(
    [Required, MaxLength(200)] string Title,
    [MaxLength(200)] string? Company,
    [MaxLength(100)] string? Location,
    [Required] string Description,
    [Url, MaxLength(500)] string? Url);

public record UpdateJobStatusDto([Required] string Status);

public record JobSkillDto(string Name, bool IsRequired);