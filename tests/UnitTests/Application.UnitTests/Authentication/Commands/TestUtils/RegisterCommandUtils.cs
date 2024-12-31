using Application.Authentication.Commands.Register;
using Bogus;
using SharedKernel.UnitTests.TestUtils;

namespace Application.UnitTests.Authentication.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="RegisterCommand"/> class.
/// </summary>
public static class RegisterCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="RegisterCommand"/> class.
    /// </summary>
    /// <param name="name">The command user name.</param>
    /// <param name="email">The command user email.</param>
    /// <param name="password">The command user password.</param>
    /// <returns>A new instance of the <see cref="RegisterCommand"/> class.</returns>
    public static RegisterCommand CreateCommand(
        string? name = null,
        string? email = null,
        string? password = null
    )
    {
        return new RegisterCommand(
            name ?? _faker.Name.FullName(),
            email ?? EmailUtils.CreateEmailAddress(),
            password ?? _faker.Internet.Password(
                length: _faker.Random.Int(6, 10),
                regexPattern: @"^(?=.*[a-z])(?=.*[0-9]).{6,}$"
            )
        );
    }
}
