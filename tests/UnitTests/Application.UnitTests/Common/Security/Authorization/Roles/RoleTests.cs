using Application.Common.Security.Authorization.Roles;

using SharedKernel.Models;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Roles;

/// <summary>
/// Unit tests for the <see cref="Role"/> class.
/// </summary>
public class RoleTests
{
    /// <summary>
    /// Tests the <see cref="Role.List"/> method returns all defined roles.
    /// </summary>
    [Fact]
    public void ListRoles_WhenCalled_ReturnsAllDefinedRoles()
    {
        var roles = Role.List();

        var expected = BaseEnumeration.GetAll<Role>();

        roles.Should().BeEquivalentTo(expected);
    }
}
