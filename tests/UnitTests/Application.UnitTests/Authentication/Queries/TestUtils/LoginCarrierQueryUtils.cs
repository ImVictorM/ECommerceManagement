using Application.Authentication.Queries.LoginCarrier;

using SharedKernel.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.Authentication.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="LoginCarrierQuery"/> class.
/// </summary>
public static class LoginCarrierQueryUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierQuery"/> class.
    /// </summary>
    /// <param name="email">The carrier email.</param>
    /// <param name="password">The carrier password.</param>
    /// <returns>A new instance of the <see cref="LoginCarrierQuery"/> class.</returns>
    public static LoginCarrierQuery CreateQuery(
        string? email = null,
        string? password = null
    )
    {
        return new LoginCarrierQuery(
            email ?? EmailUtils.CreateEmailAddress(),
            password ?? _faker.Internet.Password()
        );
    }
}
