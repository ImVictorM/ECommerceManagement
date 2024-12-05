using System.Globalization;
using Application.Orders.Commands.Common.DTOs;
using Application.Orders.Commands.PlaceOrder;
using Application.UnitTests.Orders.TestUtils.Extensions;
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
    /// <param name="userId">The owner id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <returns>A new instance of the <see cref="PlaceOrderCommand"/> class.</returns>
    public static PlaceOrderCommand CreateCommand(
        string? userId = null,
        IEnumerable<OrderProductInput>? orderProducts = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        IPaymentMethod? paymentMethod = null
    )
    {
        return new PlaceOrderCommand(
            userId ?? DomainConstants.User.Id.ToString(CultureInfo.InvariantCulture),
            orderProducts ?? DomainConstants.Order.OrderProducts.ToOrderProductInput(),
            AddressUtils.CreateAddress(),
            AddressUtils.CreateAddress(),
            PaymentUtils.CreateCreditCardPayment(),
            1
        );
    }
}
