namespace Contracts.Authentication;

/// <summary>
/// Represents a request to log in.
/// </summary>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record LoginRequest(
    string Email,
    string Password
);
