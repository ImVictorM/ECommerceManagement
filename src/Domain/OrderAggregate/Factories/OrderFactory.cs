using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate.Factories;

/// <summary>
/// Factory to create orders.
/// </summary>
public class OrderFactory
{
    private readonly IOrderProductService _orderProductService;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderFactory"/> class.
    /// </summary>
    /// <param name="orderProductService"></param>
    public OrderFactory(IOrderProductService orderProductService)
    {
        _orderProductService = orderProductService;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="ownerId">The order owner id.</param>
    /// <param name="products">The order products.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order payment billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The installments.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public async Task<Order> CreateOrder(
        UserId ownerId,
        IEnumerable<OrderProduct> products,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int? installments = null
    )
    {
        var total = await _orderProductService.CalculateTotalAsync(products);

        return Order.Create(
            ownerId,
            products,
            total,
            paymentMethod,
            billingAddress,
            deliveryAddress,
            installments
        );
    }
}
