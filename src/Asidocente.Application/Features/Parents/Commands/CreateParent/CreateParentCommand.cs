using Asidocente.Application.Common.Models;
using MediatR;

namespace Asidocente.Application.Features.Parents.Commands.CreateParent;

/// <summary>
/// Command to create a new parent
/// </summary>
public record CreateParentCommand : IRequest<Result<int>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Identification { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string? AlternatePhone { get; init; }
    public string? Occupation { get; init; }
    public string Relationship { get; init; } = string.Empty;
    public bool IsPrimaryContact { get; init; }
    public List<int> StudentIds { get; init; } = new();
}
