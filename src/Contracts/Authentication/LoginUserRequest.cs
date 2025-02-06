namespace Contracts.Authentication;

/// <summary>
/// Represents a request to log in an user.
/// </summary>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record LoginUserRequest(
    string Email,
    string Password
);
