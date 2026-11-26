
using System.ComponentModel.DataAnnotations;

namespace JobMatch.Api.Dtos;

public record ResumeDto(int Id, string Title, string Content, DateTime CreatedAt);

public record SaveResumeDto(
    [Required, MaxLength(200)] string Title,
    [Required] string Content);