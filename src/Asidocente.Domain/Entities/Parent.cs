using Asidocente.Domain.Common;

namespace Asidocente.Domain.Entities;

/// <summary>
/// Represents a parent or guardian
/// </summary>
public class Parent : BaseEntity, IAuditableEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Identification { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string? AlternatePhone { get; private set; }
    public string? Occupation { get; private set; }
    public string Relationship { get; private set; } = string.Empty; // Padre, Madre, Tutor, etc.
    public bool IsPrimaryContact { get; private set; }
    public bool IsActive { get; private set; }

    // Audit properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<Student> Students { get; private set; }

    private Parent()
    {
        Students = new List<Student>();
    }

    /// <summary>
    /// Create a new Parent
    /// </summary>
    public static Parent Create(
        string firstName,
        string lastName,
        string identification,
        string email,
        string phone,
        string relationship,
        bool isPrimaryContact = false)
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

        if (string.IsNullOrWhiteSpace(relationship))
            throw new DomainException("Relationship is required");

        var parent = new Parent
        {
            FirstName = firstName,
            LastName = lastName,
            Identification = identification,
            Email = email,
            Phone = phone,
            Relationship = relationship,
            IsPrimaryContact = isPrimaryContact,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return parent;
    }

    /// <summary>
    /// Get parent's full name
    /// </summary>
    public string GetFullName() => $"{FirstName} {LastName}";

    /// <summary>
    /// Update parent information
    /// </summary>
    public void UpdateInfo(
        string firstName,
        string lastName,
        string email,
        string phone,
        string? alternatePhone,
        string? occupation)
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
        AlternatePhone = alternatePhone;
        Occupation = occupation;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Set as primary contact
    /// </summary>
    public void SetAsPrimaryContact()
    {
        IsPrimaryContact = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Remove primary contact status
    /// </summary>
    public void RemovePrimaryContact()
    {
        IsPrimaryContact = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivate parent
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate parent
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
