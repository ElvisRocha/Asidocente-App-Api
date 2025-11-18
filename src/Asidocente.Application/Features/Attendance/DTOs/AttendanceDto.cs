namespace Asidocente.Application.Features.Attendance.DTOs;

public record AttendanceDto
{
    public int Id { get; init; }
    public DateTime AttendanceDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public TimeSpan? ArrivalTime { get; init; }
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public bool WasPresent { get; init; }
    public bool IsExcused { get; init; }
}
