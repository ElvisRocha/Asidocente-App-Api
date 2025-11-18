using Asidocente.Application.Features.Grades.DTOs;
using MediatR;

namespace Asidocente.Application.Features.Grades.Queries.GetGradesByStudent;

/// <summary>
/// Query to get grades by student
/// </summary>
public record GetGradesByStudentQuery : IRequest<List<GradeDto>>
{
    public int StudentId { get; init; }
    public int? AcademicPeriodId { get; init; }
}
