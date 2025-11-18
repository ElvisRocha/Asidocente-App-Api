using Asidocente.Domain.Common;
using Asidocente.Domain.Entities;

namespace Asidocente.Domain.Events;

/// <summary>
/// Event raised when attendance is recorded
/// </summary>
public class AttendanceRecordedEvent : IDomainEvent
{
    public Attendance Attendance { get; }
    public DateTime OccurredOn { get; }

    public AttendanceRecordedEvent(Attendance attendance)
    {
        Attendance = attendance;
        OccurredOn = DateTime.UtcNow;
    }
}
