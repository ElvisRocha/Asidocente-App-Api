using Asidocente.Domain.Common;
using System.Text.RegularExpressions;

namespace Asidocente.Domain.ValueObjects;

/// <summary>
/// Phone number value object for Costa Rican phone numbers
/// </summary>
public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    // Costa Rican phone format: 8-digit numbers (mobile and landline)
    private static readonly Regex PhoneRegex = new(
        @"^\d{8}$",
        RegexOptions.Compiled);

    public string Value { get; private set; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new PhoneNumber value object
    /// </summary>
    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new DomainException("Phone number cannot be empty");
        }

        // Remove any non-digit characters
        var cleanedNumber = Regex.Replace(phoneNumber, @"\D", "");

        if (!PhoneRegex.IsMatch(cleanedNumber))
        {
            throw new DomainException($"Phone number '{phoneNumber}' is not valid. Costa Rican numbers must be 8 digits");
        }

        return new PhoneNumber(cleanedNumber);
    }

    public bool Equals(PhoneNumber? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is PhoneNumber phoneNumber && Equals(phoneNumber);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
}
