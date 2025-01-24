using Domain.UnitTests.TestUtils;

using Application.Orders.Commands.Common.DTOs;
using Application.Orders.Commands.PlaceOrder;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;
using Bogus;

namespace Application.UnitTests.Orders.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="PlaceOrderCommand"/> command.
/// </summary>
public static class PlaceOrderCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="PlaceOrderCommand"/> class.
    /// </summary>
    /// <param name="requestId">The current request id.</param>
    /// <param name="currentUserId">The owner id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="couponsAppliedIds">The coupon applied ids.</param>
    /// <param name="installments">The installments.</param>
    /// <returns>A new instance of the <see cref="PlaceOrderCommand"/> class.</returns>
    public static PlaceOrderCommand CreateCommand(
        Guid? requestId = null,
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
            requestId ?? _faker.Random.Guid(),
            orderProducts ?? CreateOrderProductInputs(1),
            AddressUtils.CreateAddress(),
            AddressUtils.CreateAddress(),
            OrderUtils.CreateMockPaymentMethod(),
            couponsAppliedIds,
            installments
        );
    }

    /// <summary>
    /// Generates a list of <see cref="OrderProductInput"/> objects.
    /// </summary>
    /// <param name="count">The quantity of items to be generated.</param>
    /// <returns>A list of <see cref="OrderProductInput"/> objects.</returns>
    public static IEnumerable<OrderProductInput> CreateOrderProductInputs(int count = 1)
    {
        var reservedProducts = OrderUtils.CreateReservedProducts(count);

        return reservedProducts.Select(rp => new OrderProductInput(rp.ProductId.ToString(), rp.Quantity));
    }
}
