using Contracts.Common;
using Contracts.Orders;
using Contracts.Orders.Common;
using Domain.UnitTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Contracts;
using IntegrationTests.TestUtils.Seeds;

namespace IntegrationTests.Orders.TestUtils;

/// <summary>
/// Utilities for the <see cref="PlaceOrderRequest"/> class.
/// </summary>
public static class PlaceOrderRequestUtils
{
    /// <summary>
    /// Creates a new place order request object.
    /// </summary>
    /// <param name="products">The order products.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="installments">The order installments.</param>
    /// <returns>A new place order object.</returns>
    public static PlaceOrderRequest CreateRequest(
        IEnumerable<OrderProduct>? products = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        PaymentMethod? paymentMethod = null,
        int? installments = null
    )
    {
        return new PlaceOrderRequest(
            products ?? [CreateOrderProduct()],
            billingAddress ?? ContractAddressUtils.CreateAddress(),
            deliveryAddress ?? ContractAddressUtils.CreateAddress(),
            paymentMethod ?? CreateCreditCardPaymentMethod(),
            installments
        );
    }

    /// <summary>
    /// Creates a new order product.
    /// By default creates an order product for a pencil with the quantity of 1.
    /// </summary>
    /// <param name="productType">The product type.</param>
    /// <param name="quantity">The product quantity to buy.</param>
    /// <returns>A new order product.</returns>
    public static OrderProduct CreateOrderProduct(
        SeedAvailableProducts productType = SeedAvailableProducts.PENCIL,
        int quantity = 1
    )
    {
        var product = ProductSeed.GetSeedProduct(productType);

        return new OrderProduct(product.Id.ToString(), quantity);
    }

    /// <summary>
    /// Creates a new credit card payment method.
    /// </summary>
    /// <param name="token">The tokenized info about the card.</param>
    /// <returns>A new credit card payment object.</returns>
    public static CreditCardPayment CreateCreditCardPaymentMethod(string? token = null)
    {
        return new CreditCardPayment(token ?? DomainConstants.Payment.CardToken);
    }
}
