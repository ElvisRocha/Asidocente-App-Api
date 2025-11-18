namespace Asidocente.Shared.Extensions;

/// <summary>
/// Extension methods for DateTime
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Get age from date of birth
    /// </summary>
    public static int GetAge(this DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;
        return age;
    }

    /// <summary>
    /// Check if date is in the past
    /// </summary>
    public static bool IsInPast(this DateTime date)
    {
        return date < DateTime.UtcNow;
    }

    /// <summary>
    /// Check if date is in the future
    /// </summary>
    public static bool IsInFuture(this DateTime date)
    {
        return date > DateTime.UtcNow;
    }

    /// <summary>
    /// Get start of day
    /// </summary>
    public static DateTime StartOfDay(this DateTime date)
    {
        return date.Date;
    }

    /// <summary>
    /// Get end of day
    /// </summary>
    public static DateTime EndOfDay(this DateTime date)
    {
        return date.Date.AddDays(1).AddTicks(-1);
    }
}
