namespace Application.Common.Interfaces.Authentication;

/// <summary>
/// A service to hash and verify passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hash a given password.
    /// </summary>
    /// <param name="password">The password to be hashed.</param>
    /// <returns>The password hash and salt.</returns>
    (string Hash, string Salt) Hash(string password);
    /// <summary>
    /// Checks if a password equals to the given password hash.
    /// </summary>
    /// <param name="inputPassword">The password to be checked.</param>
    /// <param name="hash">The password hash to be compared.</param>
    /// <param name="salt">The password salt used to hash the password.</param>
    /// <returns>A boolean indicating if the passwords are equal.</returns>
    bool Verify(string inputPassword, string hash, string salt);
}
