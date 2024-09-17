namespace Application.Authentication.Common;

/// <summary>
/// Return type for an authenticated user.
/// </summary>
/// <param name="Id">The user id.</param>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Phone">The user phone (optional).</param>
/// <param name="Token">The user token.</param>
public record AuthenticationResult(
    long Id,
    string Name,
    string Email,
    string? Phone,
    string Token
);
