using Contracts.Users;
using Domain.UserAggregate;
using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.Users;

/// <summary>
/// Extensions for the <see cref="UserByIdResponse"/>.
/// </summary>
public static class UserByIdResponseExtensions
{
    /// <summary>
    /// Set of assertions to ensure the response user corresponds to the given user.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="user">The user to be checked against.</param>
    public static void EnsureUserCorrespondsTo(this UserByIdResponse response, User user)
    {
        response.Id.Should().Be(user.Id.ToString());
        response.Email.Should().Be(user.Email.ToString());
        response.Name.Should().Be(user.Name);
        response.Roles.Count().Should().Be(user.UserRoles.Count);
        response.Roles.Should().BeEquivalentTo(user.GetRoleNames());
        response.Phone.Should().BeEquivalentTo(user.Phone);
        response.Addresses.Count().Should().Be(user.UserAddresses.Count);
    }
}
