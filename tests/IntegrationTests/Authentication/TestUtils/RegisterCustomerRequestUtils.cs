using Domain.UnitTests.TestUtils;

using Contracts.Authentication;

using SharedKernel.UnitTests.TestUtils;

using Bogus;

namespace IntegrationTests.Authentication.TestUtils;

/// <summary>
/// Utilities related to the the <see cref="RegisterCustomerRequest"/> class.
/// </summary>
public static class RegisterCustomerRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="RegisterCustomerRequest"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="password">The user password.</param>
    /// <returns>A new instance of the <see cref="RegisterCustomerRequest"/> class.</returns>
    public static RegisterCustomerRequest CreateRequest(
        string? name = null,
        string? email = null,
        string? password = null
    )
    {
        return new RegisterCustomerRequest(
            name ?? UserUtils.CreateUserName(),
            email ?? EmailUtils.CreateEmailAddress(),
            password ?? string.Concat(
                _faker.Random.AlphaNumeric(6),
                _faker.Random.Number(0, 9)
            )
        );
    }
}
