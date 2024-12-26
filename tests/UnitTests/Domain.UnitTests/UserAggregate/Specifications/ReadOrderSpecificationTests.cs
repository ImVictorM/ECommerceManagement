using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using SharedKernel.Authorization;

namespace Domain.UnitTests.UserAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="ReadOrderSpecification"/> class.
/// Verifies the conditions under which a user has the privilege to read an order.
/// </summary>
public class ReadOrderSpecificationTests
{
    /// <summary>
    /// Ensures that a user can read their own orders.
    /// </summary>
    [Fact]
    public void ReadOrderSpecification_WhenUserIsOrderOwner_ReturnsTrue()
    {
        var orderOwnerId = UserId.Create(1);
        var currentUser = UserUtils.CreateUser(orderOwnerId);

        var specification = new ReadOrderSpecification(orderOwnerId);

        var result = specification.IsSatisfiedBy(currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Ensures that an admin user can read any order.
    /// </summary>
    [Fact]
    public void ReadOrderSpecification_WhenUserIsAdmin_ReturnsTrue()
    {
        var orderOwnerId = UserId.Create(1);
        var currentUser = UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Admin });

        var specification = new ReadOrderSpecification(orderOwnerId);

        var result = specification.IsSatisfiedBy(currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Ensures that a non-admin user cannot read another user's order.
    /// </summary>
    [Fact]
    public void ReadOrderSpecification_WhenUserIsNotOrderOwnerAndNotAdmin_ReturnsFalse()
    {
        var orderOwnerId = UserId.Create(1);
        var currentUser = UserUtils.CreateUser(UserId.Create(2), roles: new HashSet<Role> { Role.Customer });

        var specification = new ReadOrderSpecification(orderOwnerId);

        var result = specification.IsSatisfiedBy(currentUser);

        result.Should().BeFalse();
    }
}
