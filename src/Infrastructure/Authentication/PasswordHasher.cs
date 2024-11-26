using System.Security.Cryptography;
using Application.Common.Interfaces.Authentication;

namespace Infrastructure.Authentication;

/// <summary>
/// Service to hash and verify passwords.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;

    /// <inheritdoc/>
    public (string Hash, string Salt) Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            _hashAlgorithmName,
            HashSize
        );

        return (Convert.ToHexString(hash), Convert.ToHexString(salt));
    }

    /// <inheritdoc/>
    public bool Verify(string inputPassword, string hash, string salt)
    {
        var hashToCompare = Convert.FromHexString(hash);
        var saltBytes = Convert.FromHexString(salt);

        var inputPasswordHash = Rfc2898DeriveBytes.Pbkdf2(
            inputPassword,
            saltBytes,
            Iterations,
            _hashAlgorithmName,
            HashSize
        );

        return CryptographicOperations.FixedTimeEquals(hashToCompare, inputPasswordHash);
    }
}
