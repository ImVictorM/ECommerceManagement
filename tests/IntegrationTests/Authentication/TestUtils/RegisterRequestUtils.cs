using Contracts.Authentication;
using Domain.UnitTests.TestUtils.Constants;

namespace IntegrationTests.Authentication.TestUtils;

/// <summary>
/// Utilities related to the the <see cref="RegisterRequest"/> class.
/// </summary>
public static class RegisterRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="RegisterRequest"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="password">The user password.</param>
    /// <returns>A new instance of the <see cref="RegisterRequest"/> class.</returns>
    public static RegisterRequest CreateRequest(
        string? name = null,
        string? email = null,
        string? password = null
    )
    {
        return new RegisterRequest(
            name ?? DomainConstants.User.Name,
            email ?? DomainConstants.User.Email,
            password ?? DomainConstants.User.Password
        );
    }
}
