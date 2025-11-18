using Asidocente.Domain.Common;
using Asidocente.Domain.Enums;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents an academic period (trimester, bimester, semester)
/// </summary>
public class AcademicPeriod : BaseEntity, IAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public PeriodType PeriodType { get; private set; }
    public int SchoolYear { get; private set; }
    public int PeriodNumber { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }
    public int SchoolId { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public School School { get; private set; } = null!;
    public ICollection<Grade> Grades { get; private set; }

    private AcademicPeriod()
    {
        Grades = new List<Grade>();
    }

    /// <summary>
    /// Create a new AcademicPeriod
    /// </summary>
    public static AcademicPeriod Create(
        string name,
        PeriodType periodType,
        int schoolYear,
        int periodNumber,
        DateTime startDate,
        DateTime endDate,
        int schoolId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Period name is required");

        if (schoolYear < 2000 || schoolYear > 2100)
            throw new DomainException("Invalid school year");

        if (periodNumber < 1)
            throw new DomainException("Period number must be greater than zero");

        if (startDate >= endDate)
            throw new DomainException("Start date must be before end date");

        var period = new AcademicPeriod
        {
            Name = name,
            PeriodType = periodType,
            SchoolYear = schoolYear,
            PeriodNumber = periodNumber,
            StartDate = startDate,
            EndDate = endDate,
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return period;
    }

    /// <summary>
    /// Update period dates
    /// </summary>
    public void UpdateDates(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
            throw new DomainException("Start date must be before end date");

        StartDate = startDate;
        EndDate = endDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if period is currently active
    /// </summary>
    public bool IsCurrentPeriod()
    {
        var now = DateTime.UtcNow;
        return now >= StartDate && now <= EndDate && IsActive;
    }

    /// <summary>
    /// Deactivate period
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate period
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
