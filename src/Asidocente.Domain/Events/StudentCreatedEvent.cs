using Asidocente.Domain.Common;
using Asidocente.Domain.Entities;

namespace Asidocente.Domain.Events;

/// <summary>
/// Event raised when a new student is created
/// </summary>
public class StudentCreatedEvent : IDomainEvent
{
    public Student Student { get; }
    public DateTime OccurredOn { get; }

    public StudentCreatedEvent(Student student)
    {
        Student = student;
        OccurredOn = DateTime.UtcNow;
    }
}
