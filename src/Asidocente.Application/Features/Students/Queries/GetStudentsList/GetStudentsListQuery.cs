using Asidocente.Application.Common.Models;
using Asidocente.Application.Features.Students.DTOs;
using MediatR;

namespace Asidocente.Application.Features.Students.Queries.GetStudentsList;

/// <summary>
/// Query to get a paginated list of students
/// </summary>
public record GetStudentsListQuery : IRequest<PaginatedList<StudentListDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int? SchoolId { get; init; }
    public int? GradeLevel { get; init; }
    public bool? IsActive { get; init; }
    public string? SearchTerm { get; init; }
}
