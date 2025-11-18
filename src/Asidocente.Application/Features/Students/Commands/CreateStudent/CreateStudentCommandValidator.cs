using FluentValidation;

namespace Asidocente.Application.Features.Students.Commands.CreateStudent;

/// <summary>
/// Validator for CreateStudentCommand
/// </summary>
public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(v => v.Identification)
            .NotEmpty().WithMessage("Identification is required")
            .MaximumLength(20).WithMessage("Identification must not exceed 20 characters");

        RuleFor(v => v.GradeLevel)
            .IsInEnum().WithMessage("Invalid grade level");

        RuleFor(v => v.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.UtcNow.AddYears(-30)).WithMessage("Student must be younger than 30 years");

        RuleFor(v => v.SchoolId)
            .GreaterThan(0).WithMessage("School ID must be greater than 0");

        RuleFor(v => v.Email)
            .EmailAddress().When(v => !string.IsNullOrWhiteSpace(v.Email))
            .WithMessage("Invalid email address");

        RuleFor(v => v.Phone)
            .Matches(@"^\d{8}$").When(v => !string.IsNullOrWhiteSpace(v.Phone))
            .WithMessage("Phone number must be 8 digits");
    }
}
