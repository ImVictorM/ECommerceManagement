using System.Globalization;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the order.
/// </summary>
public static class OrderUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="total">The order total.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The order payment installments.</param>
    /// <returns>A new instance of the <see cref="Order"/> class.</returns>
    public static Order CreateOrder(
        UserId? userId = null,
        IEnumerable<OrderProduct>? orderProducts = null,
        decimal? total = null,
        IPaymentMethod? paymentMethod = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null
    )
    {
        return Order.Create(
            userId ?? DomainConstants.User.Id,
            orderProducts ?? Enumerable.Range(0, 5).Select(index => CreateOrderProduct(index.ToString(CultureInfo.InvariantCulture), index)),
            total ?? 120m,
            paymentMethod ?? PaymentUtils.CreateCreditCardPayment(),
            billingAddress ?? AddressUtils.CreateAddress(),
            deliveryAddress ?? AddressUtils.CreateAddress(),
            installments
        );
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="quantity">The quantity to buy.</param>
    /// <returns>A new instance of the <see cref="OrderProduct"/> class.</returns>
    public static OrderProduct CreateOrderProduct(
        string? productId = null,
        int? quantity = null
    )
    {
        return OrderProduct.Create(
            ProductId.Create(productId ?? "1"),
            quantity ?? 1
        );
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="quantity">The quantity to buy.</param>
    /// <returns>A new instance of the <see cref="OrderProduct"/> class.</returns>
    public static OrderProduct CreateOrderProduct(
        ProductId? productId = null,
        int? quantity = null
    )
    {
        return OrderProduct.Create(
           productId ?? ProductId.Create("1"),
            quantity ?? 1
        );
    }
}
