using Contracts.Orders;

using Domain.OrderAggregate;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.Orders;

/// <summary>
/// Extension methods for the <see cref="OrderDetailedResponse"/> class.
/// </summary>
public static class OrderDetailedResponseExtensions
{
    /// <summary>
    /// Ensures a <see cref="OrderDetailedResponse"/> response matches the given order.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="expectedOrder">The order the response should match.</param>
    public static void EnsureCorrespondsTo(this OrderDetailedResponse? response, Order expectedOrder)
    {
        response.Should().NotBeNull();
        response!.Id.Should().Be(expectedOrder.Id.ToString());
        response.OwnerId.Should().Be(expectedOrder.OwnerId.ToString());
        response.Description.Should().Be(expectedOrder.Description);
        response.Status.Should().Be(expectedOrder.GetStatusDescription());
        response.Payment.Should().NotBeNull();

        var responseProducts = response.Products.ToDictionary(p => p.ProductId);

        foreach (var expectedProduct in expectedOrder.Products)
        {
            var responseProduct = responseProducts[expectedProduct.ProductId.ToString()];

            responseProduct.Should().NotBeNull();
            responseProduct.Quantity.Should().Be(expectedProduct.Quantity);
            responseProduct.PurchasedPrice.Should().Be(expectedProduct.PurchasedPrice);
            responseProduct.BasePrice.Should().Be(expectedProduct.BasePrice);
        }
    }
}
