using Application.Common.Extensions;
using Application.Common.Security.Authorization.Roles;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

namespace Application.UnitTests.Common.Extensions;

/// <summary>
/// Unit tests for the <see cref="UserExtensions"/> extensions.
/// </summary>
public class UserExtensionsTests
{
    /// <summary>
    /// Define users with the expected is admin result.
    /// </summary>
    public static readonly IEnumerable<object[]> UsersWithExpectedIsAdminResult =
    [
        [
            UserUtils.CreateUser(roles: new HashSet<UserRole>()
            {
                UserRole.Create(Role.Admin.Id)
            }),
            true
        ],
        [
            UserUtils.CreateUser(roles: new HashSet<UserRole>()
            {
                UserRole.Create(Role.Customer.Id)
            }),
            false
        ]
    ];

    /// <summary>
    /// Verifies the <see cref="UserExtensions.IsAdmin(User)"/> method returns the correct boolean value.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <param name="expectedIsAdminResult">The IsAdmin expected result for the current user.</param>
    [Theory]
    [MemberData(nameof(UsersWithExpectedIsAdminResult))]
    public void IsAdmin_WhenCalled_ReturnsTheExpectedBoolean(
        User user,
        bool expectedIsAdminResult
    )
    {
        user.IsAdmin().Should().Be(expectedIsAdminResult);
    }

    /// <summary>
    /// Verifies the <see cref="UserExtensions.GetRoleNames(IEnumerable{UserRole})"/> method returns the
    /// user role names correctly.
    /// </summary>
    [Fact]
    public void GetRoleNames_WhenCalled_ReturnsRoleNames()
    {
        var user = UserUtils.CreateUser(roles: new HashSet<UserRole>()
        {
            UserRole.Create(Role.Admin.Id),
            UserRole.Create(Role.Customer.Id)
        });

        var expectedRoleNames = new List<string>()
        {
            Role.Admin.Name,
            Role.Customer.Name
        };

        var roleNames = user.UserRoles.GetRoleNames();

        roleNames.Should().BeEquivalentTo(expectedRoleNames);
    }
}
