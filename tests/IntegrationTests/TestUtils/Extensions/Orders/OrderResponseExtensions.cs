using Contracts.Orders;

using Domain.OrderAggregate;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.Orders;

/// <summary>
/// Extension methods for the <see cref="OrderResponse"/> class.
/// </summary>
public static class OrderResponseExtensions
{
    /// <summary>
    /// Verifies if an <see cref="IEnumerable{OrderResponse}"/>
    /// corresponds to the expected orders.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="expectedOrders">The expected orders.</param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<OrderResponse>? response,
        IEnumerable<Order> expectedOrders
    )
    {
        response.Should().NotBeNull();

        var responseOrders = response!.ToDictionary(o => o.Id);

        foreach (var expected in expectedOrders)
        {
            var responseOrder = responseOrders[expected.Id.ToString()];

            responseOrder.Should().NotBeNull();
            responseOrder.OwnerId.Should().Be(expected.OwnerId.ToString());
            responseOrder.Status.Should().Be(expected.OrderStatus.Name);
            responseOrder.Description.Should().Be(expected.Description);
            responseOrder.Total.Should().Be(expected.Total);
        }
    }
}
