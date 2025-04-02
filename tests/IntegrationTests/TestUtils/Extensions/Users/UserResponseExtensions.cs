using Contracts.Users;

using Domain.UserAggregate;

using SharedKernel.ValueObjects;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.Users;

/// <summary>
/// Extensions for the <see cref="UserResponse"/>.
/// </summary>
public static class UserResponseExtensions
{
    /// <summary>
    /// Ensures the response corresponds to the given user.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="expectedUser">The user to be checked against.</param>
    public static void EnsureUserCorrespondsTo(
        this UserResponse response,
        User expectedUser
    )
    {
        response.Id.Should().Be(expectedUser.Id.ToString());
        response.Email.Should().Be(expectedUser.Email.ToString());
        response.Name.Should().Be(expectedUser.Name);
        response.Roles.Count().Should().Be(expectedUser.UserRoles.Count);
        response.Roles
            .Should()
            .BeEquivalentTo(expectedUser.UserRoles.Select(ur => ur.Role.Name));
        response.Phone.Should().BeEquivalentTo(expectedUser.Phone);
        response.Addresses.Count().Should().Be(expectedUser.UserAddresses.Count);
        response.Addresses.Should().BeEquivalentTo(
            expectedUser.UserAddresses,
            opts => opts.ComparingByMembers<Address>()
        );
    }

    /// <summary>
    /// Ensures a list of user responses corresponds to the given list of
    /// expected users.
    /// </summary>
    /// <param name="responseUsers">The response user list.</param>
    /// <param name="expectedUsers">The expected users list.</param>
    public static void EnsureUsersCorrespondTo(
        this IEnumerable<UserResponse> responseUsers,
        IEnumerable<User> expectedUsers
    )
    {
        foreach (var expected in expectedUsers)
        {
            var responseUser = responseUsers
                .First(r => r.Id == expected.Id.ToString());

            responseUser.EnsureUserCorrespondsTo(expected);
        }
    }
}
