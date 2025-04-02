using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate.Factories;

/// <summary>
/// Factory responsible for orchestrating the creation of <see cref="Order"/>
/// objects.
/// </summary>
public class OrderFactory
{
    private readonly IOrderAssemblyService _orderAssemblyService;
    private readonly IOrderPricingService _orderPricingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderFactory"/> class.
    /// </summary>
    /// <param name="orderAssemblyService">
    /// The order assembly service.
    /// </param>
    /// <param name="orderPricingService">
    /// The order pricing service.
    /// </param>
    public OrderFactory(
        IOrderAssemblyService orderAssemblyService,
        IOrderPricingService orderPricingService
    )
    {
        _orderAssemblyService = orderAssemblyService;
        _orderPricingService = orderPricingService;
    }

    /// <summary>
    /// Creates a new <see cref="Order"/> by assembling line items from draft
    /// data and computing the final total.
    /// </summary>
    /// <param name="requestId">
    /// The unique identifier for the current request.
    /// </param>
    /// <param name="ownerId">
    /// The identifier of the user who owns the order.
    /// </param>
    /// <param name="shippingMethodId">
    /// The identifier of the shipping method selected for the order.
    /// </param>
    /// <param name="orderLineItemDrafts">
    /// The collection of draft line items representing the raw ordered product
    /// data.
    /// </param>
    /// <param name="paymentMethod">
    /// The payment method to be used for the order.
    /// </param>
    /// <param name="billingAddress">
    /// The billing address.
    /// </param>
    /// <param name="deliveryAddress">
    /// The delivery address.
    /// </param>
    /// <param name="installments">
    /// The number of installments for payment.
    /// </param>
    /// <param name="couponsApplied">
    /// A collection of coupons to be applied to the order.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A new instance of <see cref="Order"/> class.
    /// </returns>
    public async Task<Order> CreateOrderAsync(
        Guid requestId,
        UserId ownerId,
        ShippingMethodId shippingMethodId,
        IEnumerable<OrderLineItemDraft> orderLineItemDrafts,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int? installments = null,
        IEnumerable<OrderCoupon>? couponsApplied = null,
        CancellationToken cancellationToken = default
    )
    {
        var orderLineItems = await _orderAssemblyService.AssembleOrderLineItemsAsync(
            orderLineItemDrafts,
            cancellationToken
        );

        var total = await _orderPricingService.CalculateTotalAsync(
            orderLineItems,
            shippingMethodId,
            couponsApplied,
            cancellationToken
        );

        return Order.Create(
            requestId,
            ownerId,
            shippingMethodId,
            orderLineItems,
            total,
            paymentMethod,
            billingAddress,
            deliveryAddress,
            installments
        );
    }
}
