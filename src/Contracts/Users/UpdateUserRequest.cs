using Contracts.Users.Common;

namespace Contracts.Users;

/// <summary>
/// Represents a request object containing updated user data.
/// </summary>
/// <param name="Name">The user name.</param>
/// <param name="Phone">The user phone.</param>
/// <param name="Addresses">A list of the user addresses.</param>
/// <param name="Email">The user email address.</param>
public record UpdateUserRequest(
    string Name,
    string? Phone,
    IEnumerable<Address> Addresses,
    string Email
);
