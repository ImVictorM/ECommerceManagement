using SharedKernel.ValueObjects;

using Bogus;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="PasswordHash"/> class.
/// </summary>
public static class PasswordHashUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="PasswordHash"/> class.
    /// </summary>
    /// <param name="hash">The password hash.</param>
    /// <param name="salt">The password salt.</param>
    /// <returns>A new instance of the <see cref="PasswordHash"/> class.</returns>
    public static PasswordHash Create(
        string? hash = null,
        string? salt = null
    )
    {
        return PasswordHash.Create(
            hash ?? GenerateRandomHash(),
            salt ?? GenerateRandomSalt()
        );
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PasswordHash"/> class using
    /// a direct value.
    /// </summary>
    /// <param name="value">The password hash value.</param>
    /// <returns>A new instance of the <see cref="PasswordHash"/> class.</returns>
    public static PasswordHash CreateUsingDirectValue(string? value = null)
    {
        return PasswordHash.Create(
            value ?? $"{GenerateRandomHash()}-{GenerateRandomSalt()}"
        );
    }

    /// <summary>
    /// Generates a random hash string.
    /// </summary>
    /// <returns>A random hash string.</returns>
    public static string GenerateRandomHash()
    {
        return _faker.Random.Hexadecimal(32, "").ToUpperInvariant();
    }

    /// <summary>
    /// Generates a random salt string.
    /// </summary>
    /// <returns>A random salt string.</returns>
    public static string GenerateRandomSalt()
    {
        return _faker.Random.Hexadecimal(16, "").ToUpperInvariant();
    }
}
