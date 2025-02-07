namespace Application.Authentication.DTOs;

/// <summary>
/// Return type for an authenticated user.
/// </summary>
/// <param name="AuthenticatedIdentity">The authenticated identity.</param>
/// <param name="Token">The user token.</param>
public record AuthenticationResult(
    AuthenticatedIdentity AuthenticatedIdentity,
    string Token
);
