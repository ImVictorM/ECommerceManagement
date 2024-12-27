using Application.Orders.Commands.Common.DTOs;
using Application.Orders.Commands.PlaceOrder;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Application.UnitTests.Orders.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="PlaceOrderCommand"/> command.
/// </summary>
public static class PlaceOrderCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PlaceOrderCommand"/> class.
    /// </summary>
    /// <param name="currentUserId">The owner id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="couponsAppliedIds">The coupon applied ids.</param>
    /// <param name="installments">The installments.</param>
    /// <returns>A new instance of the <see cref="PlaceOrderCommand"/> class.</returns>
    public static PlaceOrderCommand CreateCommand(
        string? currentUserId = null,
        IEnumerable<OrderProductInput>? orderProducts = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        IPaymentMethod? paymentMethod = null,
        IEnumerable<string>? couponsAppliedIds = null,
        int? installments = null
    )
    {
        return new PlaceOrderCommand(
            currentUserId ?? DomainConstants.Order.OwnerId.ToString(),
            orderProducts ?? ToInput(DomainConstants.Order.OrderProducts),
            AddressUtils.CreateAddress(),
            AddressUtils.CreateAddress(),
            PaymentUtils.CreateCreditCardPayment(),
            couponsAppliedIds,
            installments
        );
    }

    /// <summary>
    /// Parses a list of order products to an input type.
    /// </summary>
    /// <param name="orderProducts">The order products.</param>
    /// <returns>A list of order product input.</returns>
    public static IEnumerable<OrderProductInput> ToInput(IEnumerable<OrderProduct> orderProducts)
    {
        return orderProducts.Select(op => new OrderProductInput(op.ProductId.ToString(), op.Quantity));
    }

    /// <summary>
    /// Parses a list of string coupon applied ids to order coupon.
    /// </summary>
    /// <param name="couponsAppliedIds">The ids.</param>
    /// <returns>A list of order coupon.</returns>
    public static IEnumerable<OrderCoupon> ToOrderCoupon(IEnumerable<string>? couponsAppliedIds)
    {
        if (couponsAppliedIds == null)
        {
            return [];
        }

        return couponsAppliedIds.Select(id => OrderCoupon.Create(CouponId.Create(id)));
    }
}
