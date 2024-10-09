using Domain.AddressAggregate;
using Domain.UnitTests.TestUtils.Constants;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// The address utilities for testing purposes.
/// </summary>
public static class AddressUtils
{
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
            postalCode ?? TestConstants.Address.PostalCode,
            street ?? TestConstants.Address.Street,
            neighborhood ?? TestConstants.Address.Neighborhood,
            state ?? TestConstants.Address.State,
            city ?? TestConstants.Address.City
        );
    }
}