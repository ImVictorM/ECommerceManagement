using Application.Authentication.Commands.Register;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Authentication.Commands.TestUtils;

/// <summary>
/// Register command utilities.
/// </summary>
public static class RegisterCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="RegisterCommand"/> class.
    /// </summary>
    /// <param name="name">The command user name.</param>
    /// <param name="email">The command user email.</param>
    /// <param name="password">The command user password.</param>
    /// <returns>A new instance of the <see cref="RegisterCommand"/> class.</returns>
    public static RegisterCommand CreateCommand(
        string? name = null,
        string? email = null,
        string? password = null
    )
    {
        return new RegisterCommand(
            name ?? DomainConstants.User.Name,
            email ?? SharedKernelConstants.Email.Value,
            password ?? DomainConstants.User.Password
        );
    }
}
