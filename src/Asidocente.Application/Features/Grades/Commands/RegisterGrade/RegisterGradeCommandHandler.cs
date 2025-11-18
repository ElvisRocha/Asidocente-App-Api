using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Common.Models;
using Asidocente.Domain.Entities;
using MediatR;

namespace Asidocente.Application.Features.Grades.Commands.RegisterGrade;

/// <summary>
/// Handler for RegisterGradeCommand
/// </summary>
public class RegisterGradeCommandHandler : IRequestHandler<RegisterGradeCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public RegisterGradeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(RegisterGradeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var grade = Grade.Create(
                request.Score,
                request.MaxScore,
                request.StudentId,
                request.SubjectId,
                request.AcademicPeriodId,
                request.TeacherId,
                request.Comments);

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(grade.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error registering grade: {ex.Message}");
        }
    }
}
