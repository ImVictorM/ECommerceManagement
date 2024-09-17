namespace Contracts.Authentication;

/// <summary>
/// Request type for registering a new user.
/// </summary>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record RegisterRequest(
    string Name,
    string Email,
    string Password
);
