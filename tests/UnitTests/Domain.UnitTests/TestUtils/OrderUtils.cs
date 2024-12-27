using Domain.CategoryAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Factories;
using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;

using Moq;
using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the order.
/// </summary>
public static class OrderUtils
{
    /// <summary>
    /// The order service mock.
    /// </summary>
    public static readonly Mock<IOrderService> MockOrderService = new();
    private static readonly OrderFactory _factory = new(MockOrderService.Object);

    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="id">The order id.</param>
    /// <param name="ownerId">The order owner id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The order payment installments.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public static async Task<Order> CreateOrder(
        OrderId? id = null,
        UserId? ownerId = null,
        IEnumerable<IOrderProduct>? orderProducts = null,
        IPaymentMethod? paymentMethod = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null
    )
    {

        var order = await _factory.CreateOrderAsync(
            ownerId ?? DomainConstants.User.Id,
            orderProducts ?? [
                new OrderProductInput
                {
                    ProductId = ProductId.Create(1),
                    Quantity = 1
                }
            ],
            paymentMethod ?? PaymentUtils.CreateCreditCardPayment(),
            billingAddress ?? AddressUtils.CreateAddress(),
            deliveryAddress ?? AddressUtils.CreateAddress(),
            installments
        );

        if (id != null)
        {
            order.SetIdUsingReflection(id);
        }

        return order;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="purchasedPrice">The product purchased price.</param>
    /// <param name="productCategories">The product categories.</param>
    /// <returns>A new instance of the <see cref="OrderProduct"/> class.</returns>
    public static OrderProduct CreateOrderProduct(
        ProductId? productId = null,
        int? quantity = null,
        decimal? basePrice = null,
        decimal? purchasedPrice = null,
        IReadOnlySet<CategoryId>? productCategories = null
    )
    {
        return OrderProduct.Create(
            productId ?? ProductId.Create(1),
            quantity ?? 1,
            basePrice ?? 10m,
            purchasedPrice ?? 10m,
            productCategories ?? new HashSet<CategoryId>() { CategoryId.Create(1) }
        );
    }

    /// <summary>
    /// Represents an order product input used to create orders.
    /// </summary>
    public class OrderProductInput : IOrderProduct
    {
        /// <summary>
        /// The product quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The product id.
        /// </summary>
        public ProductId ProductId { get; set; } = null!;
    }
}
