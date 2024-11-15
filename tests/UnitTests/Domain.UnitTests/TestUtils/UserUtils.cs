using System.Collections.ObjectModel;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate;
using SharedKernel.Authorization;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// User utilities.
/// </summary>
public static class UserUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="passwordHash">The password hash.</param>
    /// <param name="passwordSalt">The password salt.</param>
    /// <param name="phone">The user phone.</param>
    /// <param name="role">The user role.</param>
    /// <param name="email">The user email.</param>
    /// <param name="addresses">The user addresses.</param>
    /// <param name="active">Defines if the user should be inactive.</param>
    /// <returns>A new instance of the <see cref="User"/> class.</returns>
    public static User CreateUser(
        string? name = null,
        string? passwordHash = null,
        string? passwordSalt = null,
        string? phone = null,
        Role? role = null,
        string? email = null,
        ReadOnlyCollection<Address>? addresses = null,
        bool active = true
    )
    {
        var user = User.Create(
            name ?? DomainConstants.User.Name,
            email != null ? EmailUtils.CreateEmail(email: email) : DomainConstants.User.Email,
            passwordHash ?? DomainConstants.User.PasswordHash,
            passwordSalt ?? DomainConstants.User.PasswordSalt,
            role ?? DomainConstants.User.Role,
            phone ?? DomainConstants.User.Phone
        );

        if (addresses != null)
        {
            foreach (var address in addresses)
            {
                user.AddAddress(address);
            }
        }

        if (!active)
        {
            user.MakeInactive();
        }

        return user;
    }

    /// <summary>
    /// Creates a list of users with unique name and email.
    /// </summary>
    /// <param name="count">The quantity of users to be created.</param>
    /// <param name="active">Specify if the created users should be active or inactive.</param>
    /// <returns>A list of unique users.</returns>
    public static IEnumerable<User> CreateUsers(int count = 1, bool active = true)
    {
        var users = Enumerable
            .Range(0, count)
            .Select(index =>
                CreateUser(
                    name: DomainConstants.User.UserNameFromIndex(index),
                    email: SharedKernelConstants.Email.EmailFromIndex(index),
                    active: active
                )
            );

        return users;
    }

    /// <summary>
    /// Gets a list of invalid user name with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(string Value, Dictionary<string, string[]>)> GetInvalidNameWithCorrespondingErrors()
    {
        yield return ("", new Dictionary<string, string[]>
        {
            { "Name", [DomainConstants.User.Validations.EmptyName, DomainConstants.User.Validations.ShortName] }
        });

        yield return ("7S", new Dictionary<string, string[]>
        {
            { "Name", [DomainConstants.User.Validations.ShortName] }
        });
    }

    /// <summary>
    /// Gets a list of invalid passwords with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(string Value, Dictionary<string, string[]>)> GetInvalidPasswordWithCorrespondingErrors()
    {
        yield return (
            "",
            new Dictionary<string, string[]>
            {
                {
                    "Password",
                    [
                        DomainConstants.User.Validations.EmptyPassword,
                        DomainConstants.User.Validations.MissingCharacterPassword,
                        DomainConstants.User.Validations.MissingDigitPassword,
                        DomainConstants.User.Validations.ShortPassword
                    ]
                }
            }
         );

        yield return (
            "123456",
            new Dictionary<string, string[]>
            {
                {
                    "Password", [DomainConstants.User.Validations.MissingCharacterPassword]
                }
            }
        );

        yield return (
            "abcdef",
            new Dictionary<string, string[]>
            {
                {
                    "Password", [DomainConstants.User.Validations.MissingDigitPassword]
                }
            }
        );

        yield return (
            "a2345",
            new Dictionary<string, string[]>
            {
                {
                    "Password", [DomainConstants.User.Validations.ShortPassword]
                }
            }
        );
    }
}
