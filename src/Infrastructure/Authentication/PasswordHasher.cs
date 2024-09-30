using System.Security.Cryptography;
using Application.Common.Interfaces.Authentication;

namespace Infrastructure.Authentication;

/// <summary>
/// Service to hash and verify passwords.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// The random salt size of the hash salt.
    /// </summary>
    private const int SaltSize = 16;
    /// <summary>
    /// The hash size.
    /// </summary>
    private const int HashSize = 32;
    /// <summary>
    /// Number of iterations for the hash operation.
    /// </summary>
    private const int Iterations = 100000;

    /// <summary>
    /// The algorithm utilized in the hash process.
    /// </summary>
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;

    /// <inheritdoc/>
    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            _hashAlgorithmName,
            HashSize
        );

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    /// <inheritdoc/>
    public bool Verify(string inputPassword, string passwordHashToCompare)
    {
        string[] toCompare = passwordHashToCompare.Split("-");
        byte[] hashToCompare = Convert.FromHexString(toCompare[0]);
        byte[] salt = Convert.FromHexString(toCompare[1]);

        byte[] inputPasswordHash = Rfc2898DeriveBytes.Pbkdf2(
            inputPassword,
            salt,
            Iterations,
            _hashAlgorithmName,
            HashSize
        );

        return CryptographicOperations.FixedTimeEquals(hashToCompare, inputPasswordHash);
    }
}
