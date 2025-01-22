using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Identity;

/// <summary>
/// Unit tests for the <see cref="IdentityUserExtensions"/> class.
/// </summary>
public class IdentityUserExtensionsTests
{
    /// <summary>
    /// List containing identity users and the expected result when calling the IsAdmin method.
    /// </summary>
    public static readonly IEnumerable<object[]> IdentityUsersWithExpectedIsAdminResult =
    [
        [
            new IdentityUser("1", [Role.Admin.Name, Role.Customer.Name]),
            true,
        ],
        [
            new IdentityUser("1", [Role.Customer.Name]),
            false,
        ],
        [
            new IdentityUser("1", [Role.Admin.Name]),
            true,
        ]
    ];

    /// <summary>
    /// Tests the <see cref="IdentityUserExtensions.IsAdmin(IdentityUser)"/> returns the correct result.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <param name="expectedIsAdminResult">
    /// The expected result when calling the IsAdmin method for the current user.
    /// </param>
    [Theory]
    [MemberData(nameof(IdentityUsersWithExpectedIsAdminResult))]
    public void IsAdmin_WhenCalled_ReturnsExpectedResult(
        IdentityUser user,
        bool expectedIsAdminResult
    )
    {
        user.IsAdmin().Should().Be(expectedIsAdminResult);
    }
}
