using Domain.Users;

namespace Application.Authentication.Common;

/// <summary>
/// Return type for an authenticated user.
/// </summary>
/// <param name="user">The user authenticated.</param>
/// <param name="Token">The user token.</param>
public record AuthenticationResult(
    User User,
    string Token
);
