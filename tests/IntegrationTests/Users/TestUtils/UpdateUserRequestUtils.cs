using Contracts.Users;
using Domain.UnitTests.TestUtils.Constants;

namespace IntegrationTests.Users.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateUserRequest"/> request.
/// </summary>
public static class UpdateUserRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateUserRequest"/> class.
    /// </summary>
    /// <param name="name">The new user name.</param>
    /// <param name="phone">The new user phone.</param>
    /// <param name="email">The new user email.</param>
    /// <returns>A new instance of the <see cref="UpdateUserRequest"/> class.</returns>
    public static UpdateUserRequest CreateRequest(
        string? name = null,
        string? phone = null,
        string? email = null
    )
    {
        return new UpdateUserRequest(
            name ?? DomainConstants.User.Name,
            phone,
            email ?? DomainConstants.User.Email
        );
    }
}
