using Application.Orders.Commands.Common.DTOs;
using Domain.OrderAggregate.ValueObjects;

namespace Application.UnitTests.Orders.TestUtils.Extensions;

/// <summary>
/// Defines extension methods for the <see cref="OrderProduct"/> object.
/// </summary>
public static class OrderProductExtensions
{
    /// <summary>
    /// Converts an <see cref="OrderProduct"/> to a <see cref="OrderProductInput"/> type.
    /// </summary>
    /// <param name="orderProduct">The current order product.</param>
    /// <returns>A new instance of the <see cref="OrderProductInput"/> class.</returns>
    public static OrderProductInput ToOrderProductInput(this OrderProduct orderProduct)
    {
        return new OrderProductInput(orderProduct.ProductId.ToString(), orderProduct.Quantity);
    }

    /// <summary>
    /// Converts a list of <see cref="OrderProduct"/> to a list of <see cref="OrderProductInput"/> type.
    /// </summary>
    /// <param name="orderProduct">The current order products.</param>
    /// <returns>A new list of <see cref="OrderProductInput"/> objects.</returns>
    public static IEnumerable<OrderProductInput> ToOrderProductInput(this IEnumerable<OrderProduct> orderProduct)
    {
        foreach (var product in orderProduct)
        {
            yield return product.ToOrderProductInput();
        }
    }
}
