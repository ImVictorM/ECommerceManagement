using Application.Common.Security.Authentication;

using SharedKernel.ValueObjects;

using System.Security.Cryptography;

namespace Infrastructure.Security.Authentication;

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
    public PasswordHash Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            _hashAlgorithmName,
            HashSize
        );

        return PasswordHash.Create(Convert.ToHexString(hash), Convert.ToHexString(salt));
    }

    /// <inheritdoc/>
    public bool Verify(string inputPassword, PasswordHash passwordToCompare)
    {
        var hashToCompare = Convert.FromHexString(passwordToCompare.GetHashPart());
        var saltBytes = Convert.FromHexString(passwordToCompare.GetSaltPart());

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
