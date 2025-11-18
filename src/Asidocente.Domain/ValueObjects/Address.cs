using Asidocente.Domain.Common;

namespace Asidocente.Domain.ValueObjects;

/// <summary>
/// Address value object
/// </summary>
public sealed class Address : IEquatable<Address>
{
    public string Province { get; private set; }
    public string Canton { get; private set; }
    public string District { get; private set; }
    public string DetailedAddress { get; private set; }

    private Address(string province, string canton, string district, string detailedAddress)
    {
        Province = province;
        Canton = canton;
        District = district;
        DetailedAddress = detailedAddress;
    }

    /// <summary>
    /// Create a new Address value object for Costa Rican address
    /// </summary>
    public static Address Create(string province, string canton, string district, string detailedAddress)
    {
        if (string.IsNullOrWhiteSpace(province))
        {
            throw new DomainException("Province is required");
        }

        if (string.IsNullOrWhiteSpace(canton))
        {
            throw new DomainException("Canton is required");
        }

        if (string.IsNullOrWhiteSpace(district))
        {
            throw new DomainException("District is required");
        }

        return new Address(province, canton, district, detailedAddress ?? string.Empty);
    }

    public string GetFullAddress()
    {
        return $"{Province}, {Canton}, {District}. {DetailedAddress}";
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Province == other.Province &&
               Canton == other.Canton &&
               District == other.District &&
               DetailedAddress == other.DetailedAddress;
    }

    public override bool Equals(object? obj)
    {
        return obj is Address address && Equals(address);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Province, Canton, District, DetailedAddress);
    }

    public override string ToString() => GetFullAddress();
}
