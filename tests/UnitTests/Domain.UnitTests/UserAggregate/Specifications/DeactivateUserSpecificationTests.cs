using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Authorization;

using FluentAssertions;

namespace Domain.UnitTests.UserAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="DeactivateUserSpecification"/> class.
/// </summary>
public class DeactivateUserSpecificationTests
{
    /// <summary>
    /// Verifies that a non-admin user can deactivate themselves.
    /// </summary>
    [Fact]
    public void DeactivateUserSpecification_WhenUserIsNotAdminAndIsTargetUser_ReturnsTrue()
    {
        var currentUser = UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Customer });

        var specification = new DeactivateUserSpecification(currentUser);

        var result = specification.IsSatisfiedBy(currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that an admin user cannot deactivate themselves.
    /// </summary>
    [Fact]
    public void DeactivateUserSpecification_WhenUserIsAdminAndIsTargetUser_ReturnsFalse()
    {
        var currentUser = UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Admin });

        var specification = new DeactivateUserSpecification(currentUser);

        var result = specification.IsSatisfiedBy(currentUser);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that an admin user can deactivate another non-admin user.
    /// </summary>
    [Fact]
    public void DeactivateUserSpecification_WhenCurrentUserIsAdminAndTargetUserIsNotAdmin_ReturnsTrue()
    {
        var currentUser = UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Admin });
        var targetUser = UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Customer });

        var specification = new DeactivateUserSpecification(currentUser);
        var result = specification.IsSatisfiedBy(targetUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that a non-admin user cannot deactivate another user.
    /// </summary>
    [Fact]
    public void DeactivateUserSpecification_WhenCurrentUserIsNotAdminAndTargetUserIsNotCurrentUser_ReturnsFalse()
    {
        var currentUser = UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Customer });
        var targetUser = UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Customer });

        var specification = new DeactivateUserSpecification(currentUser);

        var result = specification.IsSatisfiedBy(targetUser);

        result.Should().BeFalse();
    }
}
