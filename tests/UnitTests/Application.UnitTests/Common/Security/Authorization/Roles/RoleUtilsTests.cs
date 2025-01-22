using Application.Common.Security.Authorization.Roles;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Roles;

/// <summary>
/// Unit tests for the <see cref="RoleUtils"/> class.
/// </summary>
public class RoleUtilsTests
{
    /// <summary>
    /// Verifies retrieving a role by its id works correctly.
    /// </summary>
    [Fact]
    public void FromValue_WithValidRoleId_ReturnsRole()
    {
        var adminRoleId = Role.Admin.Id;

        var role = RoleUtils.FromValue(adminRoleId);

        role.Should().Be(Role.Admin);
    }

    /// <summary>
    /// Verifies an exception is thrown for an invalid role id.
    /// </summary>
    [Fact]
    public void FromValue_WithInvalidRoleId_ThrowsException()
    {
        var invalidRoleId = -1;

        FluentActions
            .Invoking(() => RoleUtils.FromValue(invalidRoleId))
            .Should()
            .Throw<KeyNotFoundException>();
    }

    /// <summary>
    /// Verifies retrieving a role by its name works correctly.
    /// </summary>
    [Fact]
    public void FromDisplayName_WithValidRoleName_ReturnsRole()
    {
        var roleName = Role.Customer.Name;

        var role = RoleUtils.FromDisplayName(roleName);

        role.Should().Be(Role.Customer);
    }

    /// <summary>
    /// Verifies an exception is thrown for an invalid role name.
    /// </summary>
    [Fact]
    public void FromDisplayName_WithInvalidRoleName_ThrowsException()
    {
        var invalidRoleName = "InvalidRole";

        FluentActions
            .Invoking(() => RoleUtils.FromDisplayName(invalidRoleName))
            .Should()
            .Throw<KeyNotFoundException>();
    }
}
