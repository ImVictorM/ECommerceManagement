using Application.Authentication.Queries.Login;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Authentication.Queries.TestUtils;

/// <summary>
/// Utilities for the login query.
/// </summary>
public static class LoginQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="LoginQuery"/>.
    /// </summary>
    /// <param name="email">The input email.</param>
    /// <param name="password">The input password.</param>
    /// <returns>A new instance of the <see cref="LoginQuery"/>.</returns>
    public static LoginQuery CreateQuery(string? email = null, string? password = null)
    {
        return new LoginQuery(email ?? DomainConstants.Email.Value, password ?? DomainConstants.User.Password);
    }
}
