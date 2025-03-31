using Application.Common.Security.Identity;

using SharedKernel.ValueObjects;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Identity;

/// <summary>
/// Unit tests for the <see cref="IdentityUserExtensions"/> class.
/// </summary>
public class IdentityUserExtensionsTests
{
    /// <summary>
    /// Provides a list containing identity users and the expected result when
    /// calling the IsAdmin method.
    /// </summary>
    public static readonly IEnumerable<object[]> IdentityUsersWithExpectedIsAdminResult =
    [
        [
            new IdentityUser("1", [Role.Admin, Role.Customer]),
            true,
        ],
        [
            new IdentityUser("1", [Role.Customer]),
            false,
        ],
        [
            new IdentityUser("1", [Role.Admin]),
            true,
        ]
    ];

    /// <summary>
    /// Verifies the <see cref="IdentityUserExtensions.IsAdmin(IdentityUser)"/>
    /// returns the correct result.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <param name="expectedIsAdminResult">
    /// The expected result when calling the IsAdmin method for the current user.
    /// </param>
    [Theory]
    [MemberData(nameof(IdentityUsersWithExpectedIsAdminResult))]
    public void IsAdmin_WithDifferentUsers_ReturnsExpectedResult(
        IdentityUser user,
        bool expectedIsAdminResult
    )
    {
        user.IsAdmin().Should().Be(expectedIsAdminResult);
    }
}
