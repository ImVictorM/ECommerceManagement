using Application.Orders.Commands.Common.DTOs;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Orders.TestUtils.Extensions;

/// <summary>
/// Defines extension methods for the <see cref="OrderProductInput"/> object.
/// </summary>
public static class OrderProductInputExtensions
{
    /// <summary>
    /// Converts an <see cref="OrderProductInput"/> to <see cref="OrderProduct"/> type.
    /// </summary>
    /// <param name="input">The current order product input.</param>
    /// <returns>A new instance of the <see cref="OrderProduct"/> class.</returns>
    public static OrderProduct ToOrderProduct(this OrderProductInput input)
    {
        return OrderUtils.CreateOrderProduct(input.Id, input.Quantity);
    }

    /// <summary>
    /// Converts a list <see cref="OrderProductInput"/> to a list of <see cref="OrderProduct"/> type.
    /// </summary>
    /// <param name="inputs">The current order product inputs.</param>
    /// <returns>A new list of <see cref="OrderProduct"/> objects.</returns>
    public static IEnumerable<OrderProduct> ToOrderProduct(this IEnumerable<OrderProductInput> inputs)
    {
        foreach (var input in inputs)
        {
            yield return input.ToOrderProduct();
        }
    }
}
