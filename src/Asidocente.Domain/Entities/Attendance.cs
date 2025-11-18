using Asidocente.Domain.Common;
using Asidocente.Domain.Enums;
using Asidocente.Domain.Events;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents a student attendance record
/// </summary>
public class Attendance : BaseEntity, IAuditableEntity
{
    public DateTime AttendanceDate { get; private set; }
    public AttendanceStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public TimeSpan? ArrivalTime { get; private set; }
    public int StudentId { get; private set; }
    public int? TeacherId { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Student Student { get; private set; } = null!;
    public Teacher? Teacher { get; private set; }

    private Attendance()
    {
    }

    /// <summary>
    /// Create a new Attendance record
    /// </summary>
    public static Attendance Create(
        DateTime attendanceDate,
        AttendanceStatus status,
        int studentId,
        int? teacherId = null,
        string? notes = null,
        TimeSpan? arrivalTime = null)
    {
        var attendance = new Attendance
        {
            AttendanceDate = attendanceDate.Date,
            Status = status,
            StudentId = studentId,
            TeacherId = teacherId,
            Notes = notes,
            ArrivalTime = arrivalTime,
            CreatedAt = DateTime.UtcNow
        };

        attendance.AddDomainEvent(new AttendanceRecordedEvent(attendance));

        return attendance;
    }

    /// <summary>
    /// Update attendance status
    /// </summary>
    public void UpdateStatus(AttendanceStatus status, string? notes = null)
    {
        Status = status;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Set arrival time
    /// </summary>
    public void SetArrivalTime(TimeSpan arrivalTime)
    {
        ArrivalTime = arrivalTime;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if student was present
    /// </summary>
    public bool WasPresent()
    {
        return Status == AttendanceStatus.Present || Status == AttendanceStatus.Late;
    }

    /// <summary>
    /// Check if absence was excused
    /// </summary>
    public bool IsExcused()
    {
        return Status == AttendanceStatus.Excused || Status == AttendanceStatus.Medical;
    }
}
