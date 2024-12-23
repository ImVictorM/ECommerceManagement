using SharedKernel.UnitTests.TestUtils.Constants;
using SharedKernel.ValueObjects;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="PasswordHash"/> class.
/// </summary>
public static class PasswordHashUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PasswordHash"/> class.
    /// </summary>
    /// <param name="hash">The password hash.</param>
    /// <param name="salt">The password salt.</param>
    /// <returns>A new instance of the <see cref="PasswordHash"/> class.</returns>
    public static PasswordHash Create(
        string hash = SharedKernelConstants.PasswordHash.Hash,
        string salt = SharedKernelConstants.PasswordHash.Salt
    )
    {
        return PasswordHash.Create(hash, salt);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PasswordHash"/> class.
    /// </summary>
    /// <param name="value">The password hash value.</param>
    /// <returns>A new instance of the <see cref="PasswordHash"/> class.</returns>
    public static PasswordHash CreateUsingDirectValue(string value = SharedKernelConstants.PasswordHash.Value)
    {
        return PasswordHash.Create(value);
    }
}
