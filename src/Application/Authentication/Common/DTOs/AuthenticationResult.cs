using Domain.UserAggregate;

namespace Application.Authentication.Common.DTOs;

/// <summary>
/// Return type for an authenticated user.
/// </summary>
/// <param name="User">The user authenticated.</param>
/// <param name="Token">The user token.</param>
public record AuthenticationResult(
    User User,
    string Token
);
