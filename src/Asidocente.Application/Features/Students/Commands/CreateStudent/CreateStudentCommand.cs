using Asidocente.Application.Common.Models;
using MediatR;

namespace Asidocente.Application.Features.Students.Commands.CreateStudent;

/// <summary>
/// Command to create a new student
/// </summary>
public record CreateStudentCommand : IRequest<Result<int>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Identification { get; init; } = string.Empty;
    public int GradeLevel { get; init; }
    public DateTime DateOfBirth { get; init; }
    public int SchoolId { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Province { get; init; }
    public string? Canton { get; init; }
    public string? District { get; init; }
    public string? DetailedAddress { get; init; }
    public List<int> ParentIds { get; init; } = new();
}
