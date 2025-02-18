using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

namespace Domain.UnitTests.UserAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveUserByIdSpecification"/> class.
/// </summary>
public class QueryActiveUserByIdSpecificationTests
{
    /// <summary>
    /// Verifies that the specification returns true when the user is active and the ID matches.
    /// </summary>
    [Fact]
    public void QueryActiveUserByIdSpecification_WhenUserIsActiveAndIdMatches_ReturnsTrue()
    {
        var userId = UserId.Create(1);
        var user = UserUtils.CreateCustomer(userId, active: true);

        var specification = new QueryActiveUserByIdSpecification(userId);

        var result = specification.Criteria.Compile()(user);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns false when the user is not active.
    /// </summary>
    [Fact]
    public void QueryActiveUserByIdSpecification_WhenUserIsNotActive_ReturnsFalse()
    {
        var userId = UserId.Create(1);
        var user = UserUtils.CreateCustomer(userId, active: false);

        var specification = new QueryActiveUserByIdSpecification(userId);

        var result = specification.Criteria.Compile()(user);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification returns false when the ID does not match the user's ID.
    /// </summary>
    [Fact]
    public void QueryActiveUserByIdSpecification_WhenIdDoesNotMatch_ReturnsFalse()
    {
        var userId = UserId.Create(1);
        var user = UserUtils.CreateCustomer(UserId.Create(2), active: true);

        var specification = new QueryActiveUserByIdSpecification(userId);

        var result = specification.Criteria.Compile()(user);

        result.Should().BeFalse();
    }
}
