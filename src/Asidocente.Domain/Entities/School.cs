using Asidocente.Domain.Common;
using Asidocente.Domain.ValueObjects;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents a school institution
/// </summary>
public class School : BaseEntity, IAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string? Province { get; private set; }
    public string? Canton { get; private set; }
    public string? District { get; private set; }
    public string? DetailedAddress { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Director { get; private set; }
    public bool IsActive { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<Student> Students { get; private set; }
    public ICollection<Teacher> Teachers { get; private set; }
    public ICollection<Section> Sections { get; private set; }
    public ICollection<AcademicPeriod> AcademicPeriods { get; private set; }

    private School()
    {
        Students = new List<Student>();
        Teachers = new List<Teacher>();
        Sections = new List<Section>();
        AcademicPeriods = new List<AcademicPeriod>();
    }

    /// <summary>
    /// Create a new School
    /// </summary>
    public static School Create(
        string name,
        string code,
        string? director = null,
        string? email = null,
        string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("School name is required");

        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("School code is required");

        var school = new School
        {
            Name = name,
            Code = code,
            Director = director,
            Email = email,
            Phone = phone,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return school;
    }

    /// <summary>
    /// Update school information
    /// </summary>
    public void UpdateInfo(
        string name,
        string? director,
        string? email,
        string? phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("School name is required");

        Name = name;
        Director = director;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Set school address
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
    /// Deactivate school
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate school
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
