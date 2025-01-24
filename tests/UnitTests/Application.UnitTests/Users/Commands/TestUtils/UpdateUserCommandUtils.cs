using Application.Users.Commands.UpdateUser;

using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;

namespace Application.UnitTests.Users.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateUserCommand"/> command.
/// </summary>
public static class UpdateUserCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateUserCommand"/> class.
    /// </summary>
    /// <param name="userId">The user to be updated id.</param>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone.</param>
    /// <returns>A new instance of the <see cref="UpdateUserCommand"/> class.</returns>
    public static UpdateUserCommand CreateCommand(
        string? userId = null,
        string? name = null,
        string? email = null,
        string? phone = null
    )
    {
        return new UpdateUserCommand(
            userId ?? NumberUtils.CreateRandomLongAsString(),
            name ?? UserUtils.CreateUserName(),
            email ?? EmailUtils.CreateEmailAddress(),
            phone
        );
    }
}
