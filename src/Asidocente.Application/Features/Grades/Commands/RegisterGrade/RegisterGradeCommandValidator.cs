using FluentValidation;

namespace Asidocente.Application.Features.Grades.Commands.RegisterGrade;

/// <summary>
/// Validator for RegisterGradeCommand
/// </summary>
public class RegisterGradeCommandValidator : AbstractValidator<RegisterGradeCommand>
{
    public RegisterGradeCommandValidator()
    {
        RuleFor(v => v.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score cannot be negative");

        RuleFor(v => v.MaxScore)
            .GreaterThan(0).WithMessage("Max score must be greater than zero");

        RuleFor(v => v.Score)
            .LessThanOrEqualTo(v => v.MaxScore).WithMessage("Score cannot exceed max score");

        RuleFor(v => v.StudentId)
            .GreaterThan(0).WithMessage("Student ID is required");

        RuleFor(v => v.SubjectId)
            .GreaterThan(0).WithMessage("Subject ID is required");

        RuleFor(v => v.AcademicPeriodId)
            .GreaterThan(0).WithMessage("Academic period ID is required");
    }
}
