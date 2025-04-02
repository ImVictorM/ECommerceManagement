using Domain.CarrierAggregate;
using Domain.UserAggregate;

namespace Application.Authentication.DTOs.Results;

/// <summary>
/// Represents an authentication result.
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    /// Gets the authenticated user.
    /// </summary>
    public AuthenticatedUserResult User { get; }
    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    public string Token { get; }

    private AuthenticationResult(User user, string token)
    {
        User = AuthenticatedUserResult.FromUser(user);
        Token = token;
    }

    private AuthenticationResult(Carrier user, string token)
    {
        User = AuthenticatedUserResult.FromUser(user);
        Token = token;
    }

    internal static AuthenticationResult FromUserWithToken(
        User user,
        string token
    )
    {
        return new AuthenticationResult(user, token);
    }

    internal static AuthenticationResult FromUserWithToken(
        Carrier carrier,
        string token
    )
    {
        return new AuthenticationResult(carrier, token);
    }
}
