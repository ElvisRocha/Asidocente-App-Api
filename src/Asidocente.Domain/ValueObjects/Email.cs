using Asidocente.Domain.Common;
using System.Text.RegularExpressions;

namespace Asidocente.Domain.ValueObjects;

/// <summary>
/// Email value object
/// </summary>
public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new Email value object
    /// </summary>
    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty");
        }

        if (!EmailRegex.IsMatch(email))
        {
            throw new DomainException($"Email '{email}' is not valid");
        }

        return new Email(email.ToLowerInvariant());
    }

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Email email && Equals(email);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
