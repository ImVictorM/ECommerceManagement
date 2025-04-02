using Contracts.Common;

namespace Contracts.Users;

/// <summary>
/// Represents a user response.
/// </summary>
/// <param name="Id">The user identifier.</param>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Phone">The user phone.</param>
/// <param name="Addresses">The user addresses.</param>
/// <param name="Roles">The user role names.</param>
public record UserResponse(
    string Id,
    string Name,
    string Email,
    string? Phone,
    IEnumerable<AddressContract> Addresses,
    IEnumerable<string> Roles
);
