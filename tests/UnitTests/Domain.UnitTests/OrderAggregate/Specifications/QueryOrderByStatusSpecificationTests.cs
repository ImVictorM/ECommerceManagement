using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.OrderAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryOrderByStatusSpecification"/> specification.
/// </summary>
public class QueryOrderByStatusSpecificationTests
{
    /// <summary>
    /// Verifies the criteria resolves to true when the specification status is null.
    /// </summary>
    [Fact]
    public async Task QueryOrderByStatusSpecification_WhenStatusIsNull_ReturnsTrue()
    {
        var order = await OrderUtils.CreateOrderAsync();

        var specification = new QueryOrderByStatusSpecification(null);

        var result = specification.Criteria.Compile()(order);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the criteria resolves to true when the order status matches.
    /// </summary>
    [Fact]
    public async Task QueryOrderByStatusSpecification_WhenStatusMatches_ReturnsTrue()
    {
        var status = OrderStatus.Pending;

        var order = await OrderUtils.CreateOrderAsync(initialOrderStatus: status);

        var specification = new QueryOrderByStatusSpecification(status);

        var result = specification.Criteria.Compile()(order);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the criteria resolves to false when the order status does not match.
    /// </summary>
    [Fact]
    public async Task QueryOrderByStatusSpecification_WhenStatusDoesNotMatch_ReturnsFalse()
    {
        var specificationStatus = OrderStatus.Pending;
        var orderStatus = OrderStatus.Canceled;

        var order = await OrderUtils.CreateOrderAsync(initialOrderStatus: orderStatus);

        var specification = new QueryOrderByStatusSpecification(specificationStatus);

        var result = specification.Criteria.Compile()(order);

        result.Should().BeFalse();
    }
}
