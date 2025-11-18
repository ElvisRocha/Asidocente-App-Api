namespace Asidocente.Application.Common.Interfaces;

/// <summary>
/// DateTime service for testability
/// </summary>
public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
    DateOnly Today { get; }
}
