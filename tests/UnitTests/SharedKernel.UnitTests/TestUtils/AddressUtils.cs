using SharedKernel.ValueObjects;

using Bogus;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Address"/> class.
/// </summary>
public static class AddressUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Address"/> class.
    /// </summary>
    /// <param name="postalCode">The address postal code.</param>
    /// <param name="street">The address street code.</param>
    /// <param name="neighborhood">The address neighborhood code.</param>
    /// <param name="state">The address state code.</param>
    /// <param name="city">The address city code.</param>
    /// <returns>A new instance of the <see cref="Address"/> class.</returns>
    public static Address CreateAddress(
        string? postalCode = null,
        string? street = null,
        string? neighborhood = null,
        string? state = null,
        string? city = null
    )
    {
        return Address.Create(
            postalCode ?? _faker.Address.ZipCode(),
            street ?? _faker.Address.StreetAddress(),
            neighborhood ?? _faker.Lorem.Word(),
            state ?? _faker.Address.StateAbbr(),
            city ?? _faker.Address.City()
        );
    }
}
