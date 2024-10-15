using Domain.Common.ValueObjects;

namespace Domain.UnitTests.TestUtils;

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
        return Email.Create(email ?? Constants.DomainConstants.Email.Value);
    }
}
