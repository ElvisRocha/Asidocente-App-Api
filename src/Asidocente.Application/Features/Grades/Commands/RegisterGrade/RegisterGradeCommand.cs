using Asidocente.Application.Common.Models;
using MediatR;

namespace Asidocente.Application.Features.Grades.Commands.RegisterGrade;

/// <summary>
/// Command to register a new grade
/// </summary>
public record RegisterGradeCommand : IRequest<Result<int>>
{
    public decimal Score { get; init; }
    public decimal MaxScore { get; init; }
    public int StudentId { get; init; }
    public int SubjectId { get; init; }
    public int AcademicPeriodId { get; init; }
    public int? TeacherId { get; init; }
    public string? Comments { get; init; }
}
