using Asidocente.Application.Common.Models;
using MediatR;

namespace Asidocente.Application.Features.Attendance.Commands.RecordAttendance;

/// <summary>
/// Command to record student attendance
/// </summary>
public record RecordAttendanceCommand : IRequest<Result<int>>
{
    public DateTime AttendanceDate { get; init; }
    public int Status { get; init; }
    public int StudentId { get; init; }
    public int? TeacherId { get; init; }
    public string? Notes { get; init; }
    public TimeSpan? ArrivalTime { get; init; }
}
