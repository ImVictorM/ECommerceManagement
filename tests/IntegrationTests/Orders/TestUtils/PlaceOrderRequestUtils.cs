using Contracts.Common;
using Contracts.Common.PaymentMethods;
using Contracts.Orders;

using Domain.UnitTests.TestUtils;

using IntegrationTests.TestUtils.Contracts;

namespace IntegrationTests.Orders.TestUtils;

/// <summary>
/// Utilities for the <see cref="PlaceOrderRequest"/> class.
/// </summary>
public static class PlaceOrderRequestUtils
{
    /// <summary>
    /// Creates a new place order request object.
    /// </summary>
    /// <param name="shippingMethodId">The shipping method id.</param>
    /// <param name="products">The order products.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="couponAppliedIds">The coupon applied ids.</param>
    /// <param name="installments">The order installments.</param>
    /// <returns>A new place order object.</returns>
    public static PlaceOrderRequest CreateRequest(
        string? shippingMethodId = null,
        IEnumerable<OrderProductRequest>? products = null,
        AddressContract? billingAddress = null,
        AddressContract? deliveryAddress = null,
        PaymentMethod? paymentMethod = null,
        IEnumerable<string>? couponAppliedIds = null,
        int? installments = null
    )
    {
        var requestOrderProducts = products ?? OrderUtils
            .CreateReservedProducts(4)
            .Select(rp => new OrderProductRequest(
                rp.ProductId.ToString(),
                rp.Quantity
            ));

        return new PlaceOrderRequest(
            shippingMethodId ?? NumberUtils.CreateRandomLongAsString(),
            products ?? requestOrderProducts,
            billingAddress ?? AddressContractUtils.CreateAddress(),
            deliveryAddress ?? AddressContractUtils.CreateAddress(),
            paymentMethod ?? new CreditCardPayment("token"),
            couponAppliedIds,
            installments
        );
    }
}
