using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Common.Models;
using Asidocente.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Asidocente.Application.Features.Parents.Commands.CreateParent;

/// <summary>
/// Handler for CreateParentCommand
/// </summary>
public class CreateParentCommandHandler : IRequestHandler<CreateParentCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CreateParentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateParentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var parent = Parent.Create(
                request.FirstName,
                request.LastName,
                request.Identification,
                request.Email,
                request.Phone,
                request.Relationship,
                request.IsPrimaryContact);

            if (!string.IsNullOrWhiteSpace(request.AlternatePhone) || !string.IsNullOrWhiteSpace(request.Occupation))
            {
                parent.UpdateInfo(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Phone,
                    request.AlternatePhone,
                    request.Occupation);
            }

            // Link students if provided
            if (request.StudentIds.Any())
            {
                var students = await _context.Students
                    .Where(s => request.StudentIds.Contains(s.Id) && s.IsActive)
                    .ToListAsync(cancellationToken);

                foreach (var student in students)
                {
                    parent.Students.Add(student);
                }
            }

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(parent.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error creating parent: {ex.Message}");
        }
    }
}
