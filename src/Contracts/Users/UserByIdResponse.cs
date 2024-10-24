namespace Contracts.Users;

/// <summary>
/// The response type when getting a user by identifier.
/// </summary>
/// <param name="Id">The user identifier.</param>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Phone">The user phone.</param>
/// <param name="Addresses">The user addresses.</param>
/// <param name="Roles">The user role names.</param>
public record UserByIdResponse(
    string Id,
    string Name,
    string Email,
    string? Phone,
    IEnumerable<Address> Addresses,
    IEnumerable<string> Roles
);

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
