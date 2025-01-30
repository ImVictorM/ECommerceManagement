namespace Contracts.Authentication;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record RegisterRequest(
    string Name,
    string Email,
    string Password
);
