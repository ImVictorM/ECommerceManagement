using SharedKernel.ValueObjects;

using Bogus;
using System.Globalization;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Email"/> class.
/// </summary>
public static class EmailUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <returns>A new instance of the <see cref="Email"/> class.</returns>
    public static Email CreateEmail(string? email = null)
    {
        return Email.Create(email ?? CreateEmailAddress());
    }

    /// <summary>
    /// Creates a new unique email address.
    /// </summary>
    /// <returns>A new email address.</returns>
    public static string CreateEmailAddress()
    {
        return _faker.Internet.Email(
            firstName: _faker.Name.FirstName(),
            lastName: _faker.Name.LastName(),
            provider: "email",
            uniqueSuffix: _faker.UniqueIndex.ToString(CultureInfo.InvariantCulture)
        );
    }
}
