using Asidocente.Domain.Common;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents a teacher
/// </summary>
public class Teacher : BaseEntity, IAuditableEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Identification { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string? Specialization { get; private set; }
    public DateTime? HireDate { get; private set; }
    public bool IsActive { get; private set; }
    public int SchoolId { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public School School { get; private set; } = null!;
    public ICollection<Section> Sections { get; private set; }
    public ICollection<Subject> Subjects { get; private set; }

    private Teacher()
    {
        Sections = new List<Section>();
        Subjects = new List<Subject>();
    }

    /// <summary>
    /// Create a new Teacher
    /// </summary>
    public static Teacher Create(
        string firstName,
        string lastName,
        string identification,
        string email,
        string phone,
        int schoolId,
        string? specialization = null,
        DateTime? hireDate = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        if (string.IsNullOrWhiteSpace(identification))
            throw new DomainException("Identification is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone is required");

        var teacher = new Teacher
        {
            FirstName = firstName,
            LastName = lastName,
            Identification = identification,
            Email = email,
            Phone = phone,
            SchoolId = schoolId,
            Specialization = specialization,
            HireDate = hireDate ?? DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return teacher;
    }

    /// <summary>
    /// Get teacher's full name
    /// </summary>
    public string GetFullName() => $"{FirstName} {LastName}";

    /// <summary>
    /// Update teacher information
    /// </summary>
    public void UpdateInfo(
        string firstName,
        string lastName,
        string email,
        string phone,
        string? specialization)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone is required");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Specialization = specialization;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivate teacher
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate teacher
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
