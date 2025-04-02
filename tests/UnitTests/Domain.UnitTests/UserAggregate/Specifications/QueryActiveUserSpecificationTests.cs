using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.Specification;

using FluentAssertions;

namespace Domain.UnitTests.UserAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveUserSpecification"/> class.
/// </summary>
public class QueryActiveUserSpecificationTests
{
    /// <summary>
    /// Verifies that the specification returns true when the user is active.
    /// </summary>
    [Fact]
    public void QueryActiveUser_WithActiveUser_ReturnsTrue()
    {
        var user = UserUtils.CreateCustomer(active: true);

        var specification = new QueryActiveUserSpecification();

        var result = specification.Criteria.Compile()(user);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns false when the user is not active.
    /// </summary>
    [Fact]
    public void QueryActiveUser_WithoutActiveUser_ReturnsFalse()
    {
        var user = UserUtils.CreateCustomer(active: false);

        var specification = new QueryActiveUserSpecification();

        var result = specification.Criteria.Compile()(user);

        result.Should().BeFalse();
    }
}
