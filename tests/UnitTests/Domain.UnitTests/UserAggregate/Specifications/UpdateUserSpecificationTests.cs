using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using SharedKernel.Authorization;

namespace Domain.UnitTests.UserAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="UpdateUserSpecification"/> class.
/// Verifies the conditions under which a user is allowed to update another user.
/// </summary>
public class UpdateUserSpecificationTests
{
    /// <summary>
    /// List containing users with different roles.
    /// </summary>
    public static readonly IEnumerable<object[]> UserTypes =
    [
        [
            UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Admin })
        ],
        [
            UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Customer })
        ],
    ];

    /// <summary>
    /// Ensures that a user can update themselves.
    /// </summary>
    [Theory]
    [MemberData(nameof(UserTypes))]
    public void UpdateUserSpecification_WhenCurrentUserIsSameAsTargetUser_ReturnsTrue(User currentUser)
    {
        var specification = new UpdateUserSpecification(currentUser);

        var result = specification.IsSatisfiedBy(currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Ensures that an admin can update another user who is not an admin.
    /// </summary>
    [Fact]
    public void UpdateUserSpecification_WhenCurrentUserIsAdminAndTargetUserIsNotAdmin_ReturnsTrue()
    {
        var currentUser = UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Admin });
        var targetUser = UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Customer });

        var specification = new UpdateUserSpecification(currentUser);

        var result = specification.IsSatisfiedBy(targetUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Ensures that an admin cannot update another user who is also an admin.
    /// </summary>
    [Fact]
    public void UpdateUserSpecification_WhenCurrentUserIsAdminAndTargetUserIsAdmin_ReturnsFalse()
    {
        var currentUser = UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Admin });
        var targetUser = UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Admin });

        var specification = new UpdateUserSpecification(currentUser);

        var result = specification.IsSatisfiedBy(targetUser);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Ensures that a non-admin user cannot update another user who is not themselves.
    /// </summary>
    [Fact]
    public void UpdateUserSpecification_WhenCurrentUserIsNotAdminAndTargetUserIsNotCurrentUser_ReturnsFalse()
    {
        var currentUser = UserUtils.CreateUser(UserId.Create(1), roles: new HashSet<Role> { Role.Customer });
        var targetUser = UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Customer });

        var specification = new UpdateUserSpecification(currentUser);

        var result = specification.IsSatisfiedBy(targetUser);

        result.Should().BeFalse();
    }
}
