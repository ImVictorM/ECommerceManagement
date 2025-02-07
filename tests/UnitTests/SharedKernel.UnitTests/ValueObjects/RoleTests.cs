using SharedKernel.Models;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace SharedKernel.UnitTests.ValueObjects;

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
