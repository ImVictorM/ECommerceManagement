using Domain.OrderAggregate;
using Domain.OrderAggregate.Events;
using Domain.UnitTests.TestUtils;
using Domain.ShippingMethodAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Bogus;

namespace Application.UnitTests.TestUtils.Events.Orders;

/// <summary>
/// Utilities for the <see cref="OrderCreated"/> event.
/// </summary>
public static class OrderCreatedUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="OrderCreated"/> class.
    /// </summary>
    /// <param name="requestId">The current request id.</param>
    /// <param name="shippingMethodId">The shipping method id.</param>
    /// <param name="order">The order created.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="billingAddress">The billing address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <param name="installments">The installments quantity.</param>
    /// <returns>A new instance of the <see cref="OrderCreated"/> class.</returns>
    public static async Task<OrderCreated> CreateEventAsync(
        Guid? requestId = null,
        ShippingMethodId? shippingMethodId = null,
        Order? order = null,
        IPaymentMethod? paymentMethod = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null
    )
    {
        return new OrderCreated(
            requestId ?? _faker.Random.Guid(),
            shippingMethodId ?? ShippingMethodId.Create(NumberUtils.CreateRandomLong()),
            order ?? await OrderUtils.CreateOrderAsync(),
            paymentMethod ?? OrderUtils.CreateMockPaymentMethod(),
            deliveryAddress ?? AddressUtils.CreateAddress(),
            billingAddress ?? AddressUtils.CreateAddress(),
            installments
        );
    }
}
