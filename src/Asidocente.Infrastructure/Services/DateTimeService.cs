using Asidocente.Application.Common.Interfaces;

namespace Asidocente.Infrastructure.Services;

/// <summary>
/// DateTime service implementation
/// </summary>
public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}
