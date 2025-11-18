using Asidocente.Application.Common.Models;
using Asidocente.Application.Features.Students.DTOs;
using MediatR;

namespace Asidocente.Application.Features.Students.Queries.GetStudent;

/// <summary>
/// Query to get a student by ID
/// </summary>
public record GetStudentQuery : IRequest<Result<StudentDto>>
{
    public int Id { get; init; }
}
