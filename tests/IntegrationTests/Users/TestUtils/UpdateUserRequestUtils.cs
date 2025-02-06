using Domain.UnitTests.TestUtils;

using Contracts.Users;

using SharedKernel.UnitTests.TestUtils;

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
        string? email = null,
        string? phone = null
    )
    {
        return new UpdateUserRequest(
            name ?? UserUtils.CreateUserName(),
            email ?? EmailUtils.CreateEmailAddress(),
            phone
        );
    }
}
