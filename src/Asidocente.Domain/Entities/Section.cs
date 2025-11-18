using Asidocente.Domain.Common;
using Asidocente.Domain.Enums;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents a class section (e.g., "2-A", "3-B")
/// </summary>
public class Section : BaseEntity, IAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public GradeLevel GradeLevel { get; private set; }
    public int Capacity { get; private set; }
    public int SchoolYear { get; private set; }
    public bool IsActive { get; private set; }
    public int SchoolId { get; private set; }
    public int? HomeRoomTeacherId { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public School School { get; private set; } = null!;
    public Teacher? HomeRoomTeacher { get; private set; }
    public ICollection<Student> Students { get; private set; }

    private Section()
    {
        Students = new List<Student>();
    }

    /// <summary>
    /// Create a new Section
    /// </summary>
    public static Section Create(
        string name,
        GradeLevel gradeLevel,
        int schoolYear,
        int schoolId,
        int capacity = 30,
        int? homeRoomTeacherId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Section name is required");

        if (capacity <= 0)
            throw new DomainException("Capacity must be greater than zero");

        if (schoolYear < 2000 || schoolYear > 2100)
            throw new DomainException("Invalid school year");

        var section = new Section
        {
            Name = name,
            GradeLevel = gradeLevel,
            SchoolYear = schoolYear,
            SchoolId = schoolId,
            Capacity = capacity,
            HomeRoomTeacherId = homeRoomTeacherId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return section;
    }

    /// <summary>
    /// Update section information
    /// </summary>
    public void UpdateInfo(string name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Section name is required");

        if (capacity <= 0)
            throw new DomainException("Capacity must be greater than zero");

        Name = name;
        Capacity = capacity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Assign homeroom teacher
    /// </summary>
    public void AssignHomeRoomTeacher(int teacherId)
    {
        HomeRoomTeacherId = teacherId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if section has available capacity
    /// </summary>
    public bool HasAvailableCapacity()
    {
        return Students.Count < Capacity;
    }

    /// <summary>
    /// Get available slots
    /// </summary>
    public int GetAvailableSlots()
    {
        return Capacity - Students.Count;
    }

    /// <summary>
    /// Deactivate section
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate section
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
