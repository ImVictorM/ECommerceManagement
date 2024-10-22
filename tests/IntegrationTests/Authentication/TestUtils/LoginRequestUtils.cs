using Contracts.Authentication;
using IntegrationTests.TestUtils.Seeds;

namespace IntegrationTests.Authentication.TestUtils;

/// <summary>
/// Utilities related to the the <see cref="LoginRequest"/> class.
/// </summary>
public static class LoginRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="LoginRequest"/> class.
    /// If called with no parameters, creates a new login request object for the user1.
    /// </summary>
    /// <param name="email">The user email.</param>
    /// <param name="password">The user password.</param>
    /// <returns>A new instance of the <see cref="LoginRequest"/> class.</returns>
    public static LoginRequest CreateRequest(string? email = null, string? password = null)
    {
        return new LoginRequest(
            email ?? UserSeed.User1.Email.Value,
            password ?? UserSeed.UserPassword
        );
    }
}
