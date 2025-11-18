using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Common.Models;
using Asidocente.Domain.Entities;
using Asidocente.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Asidocente.Application.Features.Students.Commands.CreateStudent;

/// <summary>
/// Handler for CreateStudentCommand
/// </summary>
public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify school exists
            var schoolExists = await _context.Schools
                .AnyAsync(s => s.Id == request.SchoolId && s.IsActive, cancellationToken);

            if (!schoolExists)
            {
                return Result<int>.Failure("School not found or inactive");
            }

            // Check for duplicate identification
            var identificationExists = await _context.Students
                .AnyAsync(s => s.Identification == request.Identification, cancellationToken);

            if (identificationExists)
            {
                return Result<int>.Failure("A student with this identification already exists");
            }

            // Create student using factory method
            var student = Student.Create(
                request.FirstName,
                request.LastName,
                request.Identification,
                (GradeLevel)request.GradeLevel,
                request.DateOfBirth,
                request.SchoolId,
                request.Email,
                request.Phone);

            // Set address if provided
            if (!string.IsNullOrWhiteSpace(request.Province) &&
                !string.IsNullOrWhiteSpace(request.Canton) &&
                !string.IsNullOrWhiteSpace(request.District))
            {
                student.SetAddress(
                    request.Province,
                    request.Canton,
                    request.District,
                    request.DetailedAddress);
            }

            // Link parents if provided
            if (request.ParentIds.Any())
            {
                var parents = await _context.Parents
                    .Where(p => request.ParentIds.Contains(p.Id) && p.IsActive)
                    .ToListAsync(cancellationToken);

                foreach (var parent in parents)
                {
                    student.Parents.Add(parent);
                }
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(student.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error creating student: {ex.Message}");
        }
    }
}
