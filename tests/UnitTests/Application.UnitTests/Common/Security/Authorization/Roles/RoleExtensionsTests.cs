using Application.Common.Security.Authorization.Roles;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Roles;

/// <summary>
/// Unit tests for the <see cref="RoleExtensions"/> class.
/// </summary>
public class RoleExtensionsTests
{
    /// <summary>
    /// List containing roles and the expected result when calling the HasAdminRole method.
    /// </summary>
    public static readonly IEnumerable<object[]> RolesAndExpectedHasAdminRoleResult =
    [
        [
            new List<Role>()
            {
                Role.Customer,
                Role.Admin
            },
            true
        ],
        [
            new List<Role>()
            {
                Role.Customer,
            },
            false
        ],
        [
            new List<Role>()
            {
                Role.Admin,
            },
            true
        ]
    ];

    /// <summary>
    /// Verifies the <see cref="RoleExtensions.HasAdminRole(IEnumerable{Role})"/> method returns the correct result.
    /// </summary>
    /// <param name="roles">The current role list.</param>
    /// <param name="expectedHasAdminRoleResult">The expected result when calling HasAdminRole method.</param>
    [Theory]
    [MemberData(nameof(RolesAndExpectedHasAdminRoleResult))]
    public void HasAdminRole_WhenCalled_ReturnsExpectedResult(
        IReadOnlyList<Role> roles,
        bool expectedHasAdminRoleResult
    )
    {
        roles.HasAdminRole().Should().Be(expectedHasAdminRoleResult);
    }
}
