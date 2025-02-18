using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate.Factories;

/// <summary>
/// Factory to create orders.
/// </summary>
public class OrderFactory
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderFactory"/> class.
    /// </summary>
    /// <param name="orderService">The order service.</param>
    public OrderFactory(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="requestId">The current request identifier.</param>
    /// <param name="ownerId">The order owner id.</param>
    /// <param name="shippingMethodId">The shipping method id.</param>
    /// <param name="products">The order products.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order payment billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The installments.</param>
    /// <param name="couponsApplied">The coupons applied.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public async Task<Order> CreateOrderAsync(
        Guid requestId,
        UserId ownerId,
        ShippingMethodId shippingMethodId,
        IEnumerable<IOrderProductReserved> products,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int? installments = null,
        IEnumerable<OrderCoupon>? couponsApplied = null
    )
    {
        var orderProductsWithPrice = await _orderService.PrepareOrderProductsAsync(products).ToListAsync();

        var total = await _orderService.CalculateTotalAsync(orderProductsWithPrice, shippingMethodId, couponsApplied);

        return Order.Create(
            requestId,
            ownerId,
            shippingMethodId,
            orderProductsWithPrice,
            total,
            paymentMethod,
            billingAddress,
            deliveryAddress,
            installments
        );
    }
}
