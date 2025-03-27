namespace Contracts.Authentication;

/// <summary>
/// Represents a response for authentication requests.
/// </summary>
/// <param name="Id">The user identifier.</param>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Phone">The user phone (optional).</param>
/// <param name="Token">The authentication token.</param>
public record AuthenticationResponse(
    string Id,
    string Name,
    string Email,
    string? Phone,
    string Token
);
