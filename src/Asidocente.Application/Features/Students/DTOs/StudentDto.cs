namespace Asidocente.Application.Features.Students.DTOs;

public record StudentDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Identification { get; init; } = string.Empty;
    public string GradeLevel { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public int Age { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Province { get; init; }
    public string? Canton { get; init; }
    public string? District { get; init; }
    public string? DetailedAddress { get; init; }
    public bool IsActive { get; init; }
    public int SchoolId { get; init; }
    public string SchoolName { get; init; } = string.Empty;
    public int? SectionId { get; init; }
    public string? SectionName { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record StudentListDto
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Identification { get; init; } = string.Empty;
    public string GradeLevel { get; init; } = string.Empty;
    public string SchoolName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}
