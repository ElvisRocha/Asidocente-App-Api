using System.Text.RegularExpressions;

namespace Asidocente.Shared.Extensions;

/// <summary>
/// Extension methods for strings
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Convert string to title case
    /// </summary>
    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Remove special characters from string
    /// </summary>
    public static string RemoveSpecialCharacters(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return Regex.Replace(value, @"[^a-zA-Z0-9\s]", string.Empty);
    }

    /// <summary>
    /// Truncate string to specified length
    /// </summary>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
            return value;

        return value[..(maxLength - suffix.Length)] + suffix;
    }
}
