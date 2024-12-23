using SharedKernel.Errors;
using SharedKernel.Models;

namespace SharedKernel.ValueObjects;

/// <summary>
/// Represents a hashed password.
/// </summary>
public sealed class PasswordHash : ValueObject
{
    private readonly string _hash = null!;
    private readonly string _salt = null!;

    /// <summary>
    /// Gets the password hash value following the template {hash}-{salt}.
    /// </summary>
    public string Value
    {
        get => $"{_hash}-{_salt}";
    }

    private PasswordHash() { }

    private PasswordHash(string hash, string salt)
    {
        _hash = hash;
        _salt = salt;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PasswordHash"/> class.
    /// </summary>
    /// <param name="hash">The password hash.</param>
    /// <param name="salt">The password salt.</param>
    /// <returns>A new instance of the <see cref="PasswordHash"/> class.</returns>
    /// <exception cref="DomainValidationException">An exception thrown when the password hash or salt has not a valid format.</exception>
    public static PasswordHash Create(string hash, string salt)
    {
        if (
            !string.IsNullOrEmpty(hash) &&
            !string.IsNullOrEmpty(salt) &&
            IsHex(hash) &&
            IsHex(salt)
        )
        {
            return new PasswordHash(hash, salt);
        }

        throw new DomainValidationException("The hash or salt is not in a valid hexadecimal format");
    }

    /// <summary>
    /// Create a new instance of the <see cref="PasswordHash"/> class.
    /// </summary>
    /// <param name="passwordHash">The complete password hash containing hash-salt.</param>
    /// <returns>A new instance of the <see cref="PasswordHash"/> class.</returns>
    /// <exception cref="DomainValidationException">An exception thrown when the password hash has not a valid format.</exception>
    public static PasswordHash Create(string passwordHash)
    {
        var parts = passwordHash.Split("-");

        return parts.Length == 2 ?
            Create(parts[0], parts[1])
            : throw new DomainValidationException("Invalid hash and salt format");
    }

    /// <summary>
    /// Gets the password hash.
    /// </summary>
    /// <returns>The password hash.</returns>
    public string GetHashPart()
    {
        return _hash;
    }

    /// <summary>
    /// Gets the password salt.
    /// </summary>
    /// <returns>The password salt.</returns>
    public string GetSaltPart()
    {
        return _salt;
    }

    private static bool IsHex(string input)
    {
        return input.All(Uri.IsHexDigit);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
