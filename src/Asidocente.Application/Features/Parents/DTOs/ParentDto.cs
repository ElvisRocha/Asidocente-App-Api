namespace Asidocente.Application.Features.Parents.DTOs;

public record ParentDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Identification { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string? AlternatePhone { get; init; }
    public string? Occupation { get; init; }
    public string Relationship { get; init; } = string.Empty;
    public bool IsPrimaryContact { get; init; }
    public bool IsActive { get; init; }
}
