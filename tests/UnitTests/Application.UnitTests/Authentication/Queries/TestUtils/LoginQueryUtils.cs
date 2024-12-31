using Application.Authentication.Queries.Login;

using SharedKernel.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.Authentication.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="LoginQuery"/> class.
/// </summary>
public static class LoginQueryUtils
{
    private static readonly Faker _faker = new();
    /// <summary>
    /// Creates a new instance of the <see cref="LoginQuery"/>.
    /// </summary>
    /// <param name="email">The input email.</param>
    /// <param name="password">The input password.</param>
    /// <returns>A new instance of the <see cref="LoginQuery"/>.</returns>
    public static LoginQuery CreateQuery(string? email = null, string? password = null)
    {
        return new LoginQuery(
            email ?? EmailUtils.CreateEmailAddress(),
            password ?? _faker.Internet.Password()
        );
    }
}
