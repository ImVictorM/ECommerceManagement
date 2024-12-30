using Domain.OrderAggregate;
using Domain.OrderAggregate.Events;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Moq;

namespace Application.UnitTests.Orders.Events.TestUtils;

/// <summary>
/// Utilities for the <see cref="OrderCreated"/> event.
/// </summary>
public static class OrderCreatedUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrderCreated"/> class.
    /// </summary>
    /// <param name="order">The order created.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="billingAddress">The billing address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <param name="installments">The installments quantity.</param>
    /// <returns>A new instance of the <see cref="OrderCreated"/> class.</returns>
    public static async Task<OrderCreated> CreateEvent(
        Order? order = null,
        IPaymentMethod? paymentMethod = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null
    )
    {
        return new OrderCreated(
            order ?? await OrderUtils.CreateOrder(withDefaultMockSetup: true),
            paymentMethod ?? new Mock<IPaymentMethod>().Object,
            billingAddress ?? AddressUtils.CreateAddress(),
            deliveryAddress ?? AddressUtils.CreateAddress(),
            installments
        );
    }
}
