using Bogus;
using Contracts.Common;

namespace IntegrationTests.TestUtils.Contracts;

/// <summary>
/// Utilities for the <see cref="AddressContract"/> request.
/// </summary>
public static class AddressContractUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="AddressContract"/> class.
    /// </summary>
    /// <param name="postalCode">The address postal code.</param>
    /// <param name="street">The address street.</param>
    /// <param name="state">The address state.</param>
    /// <param name="city">The address city.</param>
    /// <param name="neighborhood">The address neighborhood.</param>
    /// <returns>A new instance of the <see cref="AddressContract"/> class.</returns>
    public static AddressContract CreateAddress(
        string? postalCode = null,
        string? street = null,
        string? state = null,
        string? city = null,
        string? neighborhood = null
    )
    {
        return new AddressContract(
            postalCode ?? _faker.Address.ZipCode(),
            street ?? _faker.Address.StreetAddress(),
            state ?? _faker.Address.StateAbbr(),
            city ?? _faker.Address.City(),
            neighborhood ?? _faker.Lorem.Word()
        );
    }
}
