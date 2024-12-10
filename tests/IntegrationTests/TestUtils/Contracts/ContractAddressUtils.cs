using Contracts.Common;

namespace IntegrationTests.TestUtils.Contracts;

/// <summary>
/// Utilities for the address request object.
/// </summary>
public static class ContractAddressUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Address"/> class.
    /// </summary>
    /// <param name="postalCode">The address postal code.</param>
    /// <param name="street">The address street.</param>
    /// <param name="state">The address state.</param>
    /// <param name="city">The address city.</param>
    /// <param name="neighborhood">The address neighborhood.</param>
    /// <returns>A new instance of the <see cref="Address"/> class.</returns>
    public static Address CreateAddress(
        string postalCode = "30033",
        string street = "1275 McConnell Dr",
        string state = "Georgia",
        string city = "Decatur",
        string? neighborhood = null
    )
    {
        return new Address(
            postalCode,
            street,
            state,
            city,
            neighborhood
        );
    }
}
