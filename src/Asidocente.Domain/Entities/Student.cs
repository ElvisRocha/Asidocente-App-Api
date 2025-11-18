using Asidocente.Domain.Common;
using Asidocente.Domain.Enums;
using Asidocente.Domain.Events;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents a student in the system
/// </summary>
public class Student : BaseEntity, IAuditableEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Identification { get; private set; } = string.Empty;
    public GradeLevel GradeLevel { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Province { get; private set; }
    public string? Canton { get; private set; }
    public string? District { get; private set; }
    public string? DetailedAddress { get; private set; }
    public bool IsActive { get; private set; }
    public int SchoolId { get; private set; }
    public int? SectionId { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public School School { get; private set; } = null!;
    public Section? Section { get; private set; }
    public ICollection<Parent> Parents { get; private set; }
    public ICollection<Grade> Grades { get; private set; }
    public ICollection<Attendance> Attendances { get; private set; }

    private Student()
    {
        Parents = new List<Parent>();
        Grades = new List<Grade>();
        Attendances = new List<Attendance>();
    }

    /// <summary>
    /// Create a new Student using factory method pattern
    /// </summary>
    public static Student Create(
        string firstName,
        string lastName,
        string identification,
        GradeLevel gradeLevel,
        DateTime dateOfBirth,
        int schoolId,
        string? email = null,
        string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        if (string.IsNullOrWhiteSpace(identification))
            throw new DomainException("Identification is required");

        if (dateOfBirth >= DateTime.UtcNow)
            throw new DomainException("Date of birth must be in the past");

        var student = new Student
        {
            FirstName = firstName,
            LastName = lastName,
            Identification = identification,
            GradeLevel = gradeLevel,
            DateOfBirth = dateOfBirth,
            SchoolId = schoolId,
            Email = email,
            Phone = phone,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        student.AddDomainEvent(new StudentCreatedEvent(student));

        return student;
    }

    /// <summary>
    /// Get student's full name
    /// </summary>
    public string GetFullName() => $"{FirstName} {LastName}";

    /// <summary>
    /// Update student information
    /// </summary>
    public void UpdateInfo(
        string firstName,
        string lastName,
        string? email,
        string? phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update grade level (promotion)
    /// </summary>
    public void UpdateGradeLevel(GradeLevel newLevel)
    {
        GradeLevel = newLevel;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Assign to section
    /// </summary>
    public void AssignToSection(int sectionId)
    {
        SectionId = sectionId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Set student address
    /// </summary>
    public void SetAddress(string province, string canton, string district, string? detailedAddress)
    {
        Province = province;
        Canton = canton;
        District = district;
        DetailedAddress = detailedAddress;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivate student
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate student
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculate student's age
    /// </summary>
    public int GetAge()
    {
        var today = DateTime.UtcNow;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}
