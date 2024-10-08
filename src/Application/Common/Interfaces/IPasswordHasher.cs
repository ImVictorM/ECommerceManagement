namespace Application.Common.Interfaces;

/// <summary>
/// A service to hash and verify passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hash a given password.
    /// </summary>
    /// <param name="password">The password to be hashed.</param>
    /// <returns>The password hash.</returns>
    string Hash(string password);
    /// <summary>
    /// Checks if a password equals to the given password hash.
    /// </summary>
    /// <param name="inputPassword">The password to be checked.</param>
    /// <param name="passwordHashToCompare">The password hash to be compared.</param>
    /// <returns>A boolean indicating if the passwords are equal.</returns>
    bool Verify(string inputPassword, string passwordHashToCompare);
}
