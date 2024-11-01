namespace Contracts.Users.Common;

/// <summary>
/// Represents an address response.
/// </summary>
/// <param name="PostalCode">The address postal code.</param>
/// <param name="Street">The address street.</param>
/// <param name="Neighborhood">The address neighborhood.</param>
/// <param name="State">The address state.</param>
/// <param name="City">The address city.</param>
public record Address(
    string PostalCode,
    string Street,
    string? Neighborhood,
    string State,
    string City
);
