namespace Contracts.Authentication;

/// <summary>
/// Request type to login a registered user.
/// </summary>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record LoginRequest(
    string Email,
    string Password
);
