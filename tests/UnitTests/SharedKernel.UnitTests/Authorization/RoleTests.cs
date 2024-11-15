using FluentAssertions;
using SharedKernel.Authorization;

namespace SharedKernel.UnitTests.Authorization;

/// <summary>
/// Unit tests for the <see cref="Role"/> class, verifying role retrieval, validation, and filtering behavior.
/// </summary>
public class RoleTests
{
    /// <summary>
    /// Verifies that <see cref="Role.GetRoleByName"/> returns the correct role when a valid role name is provided.
    /// </summary>
    [Fact]
    public void Role_WhenRoleNameIsValid_GetRoleByNameShouldReturnCorrectRole()
    {
        var role = Role.GetRoleByName("admin");

        role.Should().NotBeNull();
        role.Should().Be(Role.Admin);
    }

    /// <summary>
    /// Verifies that <see cref="Role.GetRoleByName"/> throws an <see cref="ArgumentException"/> when an invalid role name is provided.
    /// </summary>
    [Fact]
    public void Role_WhenRoleNameIsInvalid_ShouldThrowError()
    {
        FluentActions
            .Invoking(() => Role.GetRoleByName("invalid"))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("invalid is not a valid role name");
    }

    /// <summary>
    /// Verifies that <see cref="Role.HasAdminRole(IEnumerable{string})"/> returns true when the role name list contains "admin".
    /// </summary>
    [Fact]
    public void Role_WhenRoleNameListContainsAdmin_HasAdminRoleShouldReturnTrue()
    {
        var roleNames = new List<string> { "admin", "customer" };

        var result = Role.HasAdminRole(roleNames);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that <see cref="Role.HasAdminRole(IEnumerable{string})"/> returns false when the role name list does not contain "admin".
    /// </summary>
    [Fact]
    public void Role_WhenRoleNameListDoesNotContainAdmin_HasAdminRoleShouldReturnFalse()
    {
        var roleNames = new List<string> { "customer" };

        var result = Role.HasAdminRole(roleNames);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that <see cref="Role.HasAdminRole(IEnumerable{long})"/> returns true when the role id list contains the Admin role id.
    /// </summary>
    [Fact]
    public void Role_WhenRoleIdListContainsAdminId_HasAdminRoleShouldReturnTrue()
    {
        // 1 is the Admin role id
        var roleIds = new List<long> { 1, 2 };

        var result = Role.HasAdminRole(roleIds);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that <see cref="Role.HasAdminRole(IEnumerable{long})"/> returns false when the role id list does not contain the Admin role id.
    /// </summary>
    [Fact]
    public void Role_WhenRoleIdListDoesNotContainAdminId_HasAdminRoleShouldReturnFalse_()
    {
        var roleIds = new List<long> { 2 };

        var result = Role.HasAdminRole(roleIds);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that <see cref="Role.List()"/> returns all defined roles.
    /// </summary>
    [Fact]
    public void Role_WhenListingAllRoles_ShouldReturnAllDefinedRoles()
    {
        var roles = Role.List();

        Assert.Contains(Role.Admin, roles);
        Assert.Contains(Role.Customer, roles);

        roles.Should().Contain(Role.Admin);
        roles.Should().Contain(Role.Customer);
        roles.Count().Should().Be(2);
    }

    /// <summary>
    /// Verifies that <see cref="Role.List(Func{Role, bool})"/> returns a filtered list of roles based on the provided predicate.
    /// </summary>
    [Fact]
    public void Role_WhenListingRolesWithFilter_ShouldReturnFilteredRoles()
    {
        var roles = Role.List(role => role.Id == 1).ToList();

        roles.Count.Should().Be(1);
        roles.Should().Contain(Role.Admin);
    }
}
