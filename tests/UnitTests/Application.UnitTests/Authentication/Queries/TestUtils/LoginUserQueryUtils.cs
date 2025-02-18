using Application.Authentication.Queries.LoginUser;

using SharedKernel.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.Authentication.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="LoginUserQuery"/> class.
/// </summary>
public static class LoginUserQueryUtils
{
    private static readonly Faker _faker = new();
    /// <summary>
    /// Creates a new instance of the <see cref="LoginUserQuery"/>.
    /// </summary>
    /// <param name="email">The input email.</param>
    /// <param name="password">The input password.</param>
    /// <returns>A new instance of the <see cref="LoginUserQuery"/>.</returns>
    public static LoginUserQuery CreateQuery(string? email = null, string? password = null)
    {
        return new LoginUserQuery(
            email ?? EmailUtils.CreateEmailAddress(),
            password ?? _faker.Internet.Password()
        );
    }
}
