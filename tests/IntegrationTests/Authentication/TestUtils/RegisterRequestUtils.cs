using Contracts.Authentication;

using SharedKernel.UnitTests.TestUtils;

using Bogus;

namespace IntegrationTests.Authentication.TestUtils;

/// <summary>
/// Utilities related to the the <see cref="RegisterRequest"/> class.
/// </summary>
public static class RegisterRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="RegisterRequest"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="password">The user password.</param>
    /// <returns>A new instance of the <see cref="RegisterRequest"/> class.</returns>
    public static RegisterRequest CreateRequest(
        string? name = null,
        string? email = null,
        string? password = null
    )
    {
        return new RegisterRequest(
            name ?? _faker.Name.FullName(),
            email ?? EmailUtils.CreateEmailAddress(),
            password ?? string.Concat(
                _faker.Random.AlphaNumeric(6),
                _faker.Random.Number(0, 9)
            )
        );
    }
}
