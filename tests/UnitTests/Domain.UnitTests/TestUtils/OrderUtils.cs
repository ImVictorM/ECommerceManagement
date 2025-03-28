using Domain.CategoryAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Factories;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Domain.OrderAggregate.Enumerations;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

using Moq;
using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Order"/> class.
/// </summary>
public static class OrderUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new fake instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="requestId">The request identifier.</param>
    /// <param name="id">The order identifier.</param>
    /// <param name="ownerId">The order owner identifier.</param>
    /// <param name="shippingMethodId">The shipping method identifier.</param>
    /// <param name="orderLineItemDrafts">The order line item drafts.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The order installments.</param>
    /// <param name="couponsApplied">The order coupons applied.</param>
    /// <param name="initialOrderStatus">The initial order status.</param>
    /// <returns>A new fake instance of the <see cref="Order"/> class.</returns>
    public static async Task<Order> CreateOrderAsync(
        Guid? requestId = null,
        OrderId? id = null,
        UserId? ownerId = null,
        ShippingMethodId? shippingMethodId = null,
        IEnumerable<OrderLineItemDraft>? orderLineItemDrafts = null,
        IPaymentMethod? paymentMethod = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null,
        IEnumerable<OrderCoupon>? couponsApplied = null,
        OrderStatus? initialOrderStatus = null
    )
    {
        var lineItemDrafts = orderLineItemDrafts ?? CreateOrderLineItemDrafts();

        var factory = CreateMockedOrderFactory(lineItemDrafts);

        var order = await factory.CreateOrderAsync(
            requestId ?? _faker.Random.Guid(),
            ownerId ?? UserId.Create(_faker.Random.Long()),
            shippingMethodId ?? ShippingMethodId.Create(_faker.Random.Long()),
            lineItemDrafts,
            paymentMethod ?? CreateMockPaymentMethod(),
            billingAddress ?? AddressUtils.CreateAddress(),
            deliveryAddress ?? AddressUtils.CreateAddress(),
            installments,
            couponsApplied
        );

        if (id != null)
        {
            order.SetIdUsingReflection(id);
        }

        if (initialOrderStatus is not null)
        {
            var statusProperty = typeof(Order).GetProperty(
                nameof(Order.OrderStatus),
                System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance
            );

            if (statusProperty != null && statusProperty.CanWrite)
            {
                statusProperty.SetValue(order, initialOrderStatus);
            }
        }

        return order;
    }

    /// <summary>
    /// Creates a single instance of <see cref="OrderLineItemDraft"/>.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantity">The product quantity.</param>
    /// <returns>A new instance of <see cref="OrderLineItemDraft"/>.</returns>
    public static OrderLineItemDraft CreateOrderLineItemDraft(
        ProductId? productId = null,
        int? quantity = null
    )
    {
        return OrderLineItemDraft.Create(
            productId ?? ProductId.Create(_faker.Random.Long()),
            quantity ?? _faker.Random.Int(1, 20)
        );
    }

    /// <summary>
    /// Creates a collection of <see cref="OrderLineItemDraft"/>.
    /// </summary>
    /// <param name="count">
    /// The quantity of order line item drafts to be created.
    /// </param>
    /// <returns>A collection of <see cref="OrderLineItemDraft"/>.</returns>
    public static IReadOnlyCollection<OrderLineItemDraft> CreateOrderLineItemDrafts(
        int count = 1
    )
    {
        return Enumerable
            .Range(0, count)
            .Select(i => CreateOrderLineItemDraft(
                productId: ProductId.Create(i + 1)
            ))
            .ToList();
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderLineItem"/>.
    /// </summary>
    /// <param name="productId">The line item product identifier.</param>
    /// <param name="quantity">The quantity purchased.</param>
    /// <param name="basePrice">The line item base price.</param>
    /// <param name="purchasedPrice">The line item purchased price.</param>
    /// <param name="productCategories">The line item product categories.</param>
    /// <returns>A new instance of <see cref="OrderLineItem"/>.</returns>
    public static OrderLineItem CreateOrderLineItem(
        ProductId? productId = null,
        int? quantity = null,
        decimal? basePrice = null,
        decimal? purchasedPrice = null,
        IReadOnlySet<CategoryId>? productCategories = null
    )
    {
        var lineItemBasePrice = _faker.Finance.Amount(10, 100);
        var lineItemPurchasePrice = lineItemBasePrice * _faker.Random.Decimal(0.5m, 1m);

        return OrderLineItem.Create(
            productId ?? ProductId.Create(_faker.Random.Int(1, 1000)),
            quantity ?? _faker.Random.Int(1, 10),
            basePrice ?? lineItemBasePrice,
            purchasedPrice ?? lineItemPurchasePrice,
            productCategories ?? new HashSet<CategoryId>
            {
                CategoryId.Create(_faker.Random.Int(1, 10))
            }
        );
    }

    /// <summary>
    /// Creates a collection of <see cref="OrderLineItem"/>.
    /// </summary>
    /// <param name="count">The quantity of line items to be created.</param>
    /// <returns>A collection of <see cref="OrderLineItem"/>.</returns>
    public static IReadOnlyCollection<OrderLineItem> CreateOrderLineItems(
        int count = 1
    )
    {
        return Enumerable
            .Range(0, count)
            .Select(i => CreateOrderLineItem(
                productId: ProductId.Create(i + 1)
            ))
            .ToList();
    }

    /// <summary>
    /// Creates a collection of <see cref="OrderCoupon"/>.
    /// </summary>
    /// <param name="count">The quantity of order coupons to be created.</param>
    /// <returns>A collection of <see cref="OrderCoupon"/>.</returns>
    public static IReadOnlyCollection<OrderCoupon> CreateOrderCoupons(
        int count = 1
    )
    {
        return Enumerable
            .Range(0, count)
            .Select(i => OrderCoupon.Create(
                CouponId.Create(i + 1)
            ))
            .ToList();
    }

    /// <summary>
    /// Creates a new instance of <see cref="IPaymentMethod"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="IPaymentMethod"/>.</returns>
    public static IPaymentMethod CreateMockPaymentMethod()
    {
        var mock = new Mock<IPaymentMethod>();

        mock.SetupGet(pm => pm.Type).Returns("mock_payment");

        return mock.Object;
    }

    private static OrderFactory CreateMockedOrderFactory(
        IEnumerable<OrderLineItemDraft> lineItemDrafts
    )
    {
        var lineItems = lineItemDrafts.Select(draft => CreateOrderLineItem(
            draft.ProductId,
            draft.Quantity
        ));

        var total = lineItems.Sum(pp => pp.CalculateTransactionPrice());

        var mockAssemblyService = new Mock<IOrderAssemblyService>();
        var mockPricingService = new Mock<IOrderPricingService>();

        mockAssemblyService
            .Setup(s => s.AssembleOrderLineItemsAsync(
                lineItemDrafts,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(lineItems);

        mockPricingService
            .Setup(s => s.CalculateTotalAsync(
                lineItems,
                It.IsAny<ShippingMethodId>(),
                It.IsAny<IEnumerable<OrderCoupon>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(total);

        return new OrderFactory(
            mockAssemblyService.Object,
            mockPricingService.Object
        );
    }
}
