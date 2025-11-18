using Asidocente.Domain.Common;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents a subject/course
/// </summary>
public class Subject : BaseEntity, IAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Credits { get; private set; }
    public bool IsActive { get; private set; }
    public int SchoolId { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public School School { get; private set; } = null!;
    public ICollection<Grade> Grades { get; private set; }
    public ICollection<Teacher> Teachers { get; private set; }

    private Subject()
    {
        Grades = new List<Grade>();
        Teachers = new List<Teacher>();
    }

    /// <summary>
    /// Create a new Subject
    /// </summary>
    public static Subject Create(
        string name,
        string code,
        int schoolId,
        string? description = null,
        int credits = 1)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Subject name is required");

        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Subject code is required");

        if (credits < 0)
            throw new DomainException("Credits cannot be negative");

        var subject = new Subject
        {
            Name = name,
            Code = code,
            SchoolId = schoolId,
            Description = description,
            Credits = credits,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return subject;
    }

    /// <summary>
    /// Update subject information
    /// </summary>
    public void UpdateInfo(string name, string? description, int credits)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Subject name is required");

        if (credits < 0)
            throw new DomainException("Credits cannot be negative");

        Name = name;
        Description = description;
        Credits = credits;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivate subject
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate subject
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
