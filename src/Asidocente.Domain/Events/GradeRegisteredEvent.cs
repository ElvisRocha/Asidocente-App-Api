using Asidocente.Domain.Common;
using Asidocente.Domain.Entities;

namespace Asidocente.Domain.Events;

/// <summary>
/// Event raised when a grade is registered
/// </summary>
public class GradeRegisteredEvent : IDomainEvent
{
    public Grade Grade { get; }
    public DateTime OccurredOn { get; }

    public GradeRegisteredEvent(Grade grade)
    {
        Grade = grade;
        OccurredOn = DateTime.UtcNow;
    }
}
