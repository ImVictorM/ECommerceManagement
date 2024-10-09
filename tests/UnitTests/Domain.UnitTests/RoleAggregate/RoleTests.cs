using Domain.RoleAggregate;
using Domain.RoleAggregate.Enums;
using FluentAssertions;

namespace Domain.UnitTests.RoleAggregate;

/// <summary>
/// Test for the <see cref="Role"/> aggregate root.
/// </summary>
public class RoleTests
{
    /// <summary>
    /// Lists the role type and its related name.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> RoleTypeAndNamePairs()
    {
        yield return new object[] { RoleTypes.CUSTOMER, "customer" };
        yield return new object[] { RoleTypes.ADMIN, "admin", };
    }

    /// <summary>
    /// Tests if it creates a role correctly based on its type.
    /// </summary>
    /// <param name="roleType">The role type.</param>
    /// <param name="expectedName">The role expected name by type.</param>
    [Theory]
    [MemberData(nameof(RoleTypeAndNamePairs))]
    public void Role_WhenCreatingWithValidRoleType_ShouldReturnRoleWithCorrectName(RoleTypes roleType, string expectedName)
    {
        var role = Role.Create(roleType);

        role.Should().NotBeNull();
        role.Name.Should().Be(expectedName);
    }

    /// <summary>
    /// Tests if it can parse a role type to its respective name correctly.
    /// </summary>
    /// <param name="roleType">The role type.</param>
    /// <param name="expectedName">The role expected name by type.</param>
    [Theory]
    [MemberData(nameof(RoleTypeAndNamePairs))]
    public void Role_WhenParsingRoleTypeToName_ShouldReturnCorrectName(RoleTypes roleType, string expectedName)
    {
        var result = Role.ToName(roleType);

        result.Should().Be(expectedName);
    }
}
