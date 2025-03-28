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
    /// Verifies that the specification returns true when the user is active
    /// and the identifier matches.
    /// </summary>
    [Fact]
    public void QueryActiveUserById_WithActiveMatchingId_ReturnsTrue()
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
    public void QueryActiveUserById_WithInactiveUser_ReturnsFalse()
    {
        var userId = UserId.Create(1);
        var user = UserUtils.CreateCustomer(userId, active: false);

        var specification = new QueryActiveUserByIdSpecification(userId);

        var result = specification.Criteria.Compile()(user);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification returns false when the identifier does
    /// not match the user's identifier.
    /// </summary>
    [Fact]
    public void QueryActiveUserById_WithoutMatchingId_ReturnsFalse()
    {
        var userId = UserId.Create(1);
        var user = UserUtils.CreateCustomer(UserId.Create(2), active: true);

        var specification = new QueryActiveUserByIdSpecification(userId);

        var result = specification.Criteria.Compile()(user);

        result.Should().BeFalse();
    }
}
