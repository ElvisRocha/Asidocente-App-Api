using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Common.Models;
using Asidocente.Domain.Entities;
using Asidocente.Domain.Enums;
using MediatR;

namespace Asidocente.Application.Features.Attendance.Commands.RecordAttendance;

/// <summary>
/// Handler for RecordAttendanceCommand
/// </summary>
public class RecordAttendanceCommandHandler : IRequestHandler<RecordAttendanceCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public RecordAttendanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(RecordAttendanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var attendance = Domain.Entities.Attendance.Create(
                request.AttendanceDate,
                (AttendanceStatus)request.Status,
                request.StudentId,
                request.TeacherId,
                request.Notes,
                request.ArrivalTime);

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(attendance.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error recording attendance: {ex.Message}");
        }
    }
}
