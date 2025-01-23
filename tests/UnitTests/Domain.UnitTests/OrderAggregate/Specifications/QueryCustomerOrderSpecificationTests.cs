using Domain.OrderAggregate.Specifications;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

namespace Domain.UnitTests.OrderAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryCustomerOrderSpecification"/> specification.
/// </summary>
public class QueryCustomerOrderSpecificationTests
{
    /// <summary>
    /// Verifies the criteria returns true when the ids match.
    /// </summary>
    [Fact]
    public async Task QueryCustomerOrderSpecification_WhenUserIdMatchesOrderOwnerId_ReturnsTrue()
    {
        var orderOwnerId = UserId.Create(1);
        var order = await OrderUtils.CreateOrderAsync(id: OrderId.Create(1), ownerId: orderOwnerId);

        var specification = new QueryCustomerOrderSpecification(orderOwnerId);

        var result = specification.Criteria.Compile()(order);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the criteria returns false when the ids do not match.
    /// </summary>
    [Fact]
    public async Task QueryCustomerOrderSpecification_WhenUserIdDoesNotMatchOrderOwnerId_ReturnsFalse()
    {
        var orderOwnerId = UserId.Create(1);
        var otherUserId = UserId.Create(2);
        var order = await OrderUtils.CreateOrderAsync(id: OrderId.Create(1), ownerId: orderOwnerId);

        var specification = new QueryCustomerOrderSpecification(otherUserId);

        var result = specification.Criteria.Compile()(order);

        result.Should().BeFalse();
    }
}
