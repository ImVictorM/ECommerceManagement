using Application.Orders.Commands.PlaceOrder;
using Application.Orders.DTOs;

using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Bogus;

namespace Application.UnitTests.Orders.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="PlaceOrderCommand"/> command.
/// </summary>
public static class PlaceOrderCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="PlaceOrderCommand"/> class.
    /// </summary>
    /// <param name="requestId">The current request id.</param>
    /// <param name="shippingMethodId">The shipping method id.</param>
    /// <param name="products">The order products.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="couponsAppliedIds">The coupon applied ids.</param>
    /// <param name="installments">The installments.</param>
    /// <returns>
    /// A new instance of the <see cref="PlaceOrderCommand"/> class.
    /// </returns>
    public static PlaceOrderCommand CreateCommand(
        Guid? requestId = null,
        string? shippingMethodId = null,
        IEnumerable<OrderLineItemInput>? products = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        IPaymentMethod? paymentMethod = null,
        IEnumerable<string>? couponsAppliedIds = null,
        int? installments = null
    )
    {
        return new PlaceOrderCommand(
            requestId ?? _faker.Random.Guid(),
            shippingMethodId ?? NumberUtils.CreateRandomLongAsString(),
            products ?? CreateOrderLineItemInputs(count: 2),
            AddressUtils.CreateAddress(),
            AddressUtils.CreateAddress(),
            OrderUtils.CreateMockPaymentMethod(),
            couponsAppliedIds,
            installments
        );
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderLineItemInput"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="quantity">The product quantity.</param>
    /// <returns>
    /// A new instance of the <see cref="OrderLineItemInput"/> class.
    /// </returns>
    public static OrderLineItemInput CreateOrderLineItemInput(
        string? productId = null,
        int? quantity = null
    )
    {
        return new OrderLineItemInput(
            productId ?? NumberUtils.CreateRandomLongAsString(),
            quantity ?? _faker.Random.Int(1, 10)
        );
    }

    /// <summary>
    /// Creates a collection of <see cref="OrderLineItemInput"/>.
    /// </summary>
    /// <param name="count">
    /// The quantity of order line item input to be created.
    /// </param>
    /// <returns>A collection of <see cref="OrderLineItemInput"/>.</returns>
    public static IReadOnlyCollection<OrderLineItemInput> CreateOrderLineItemInputs(
        int count = 1
    )
    {
        return Enumerable
            .Range(0, count)
            .Select(i => CreateOrderLineItemInput(
                productId: $"{i + 1}"
            ))
            .ToList();
    }
}
