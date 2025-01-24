using Application.Orders.Commands.Common.DTOs;

using Domain.OrderAggregate.Interfaces;

namespace Application.UnitTests.Orders.TestUtils.Extensions;

/// <summary>
/// Extension methods for the order product reserved.
/// </summary>
public static class OrderProductReservedExtensions
{
    /// <summary>
    /// Converts a list of <see cref="IOrderProductReserved"/> to an implementation of <see cref="OrderProductInput"/>.
    /// </summary>
    /// <param name="reservedProducts">The current reserved products.</param>
    /// <returns>A list of <see cref="OrderProductInput"/> items.</returns>
    public static IEnumerable<OrderProductInput> ParseToInput(this IEnumerable<IOrderProductReserved> reservedProducts)
    {
        return reservedProducts
            .Select(rp => new OrderProductInput(rp.ProductId.ToString(), rp.Quantity));
    }
}
