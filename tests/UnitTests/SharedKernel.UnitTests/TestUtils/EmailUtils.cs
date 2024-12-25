using SharedKernel.UnitTests.TestUtils.Constants;
using SharedKernel.ValueObjects;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Email utilities.
/// </summary>
public static class EmailUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <returns>A new instance of the <see cref="Email"/> class.</returns>
    public static Email CreateEmail(string? email = null)
    {
        return Email.Create(email ?? SharedKernelConstants.Email.Value);
    }

    /// <summary>
    /// Returns an email concatenated with an index.
    /// </summary>
    /// <param name="index">The index to concatenate with.</param>
    /// <returns>A new email with a concatenated index.</returns>
    public static Email EmailFromIndex(int index)
    {
        return CreateEmail($"{index}{SharedKernelConstants.Email.Value}");
    }
}
