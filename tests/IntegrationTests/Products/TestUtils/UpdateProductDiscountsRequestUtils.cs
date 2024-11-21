using Contracts.Products;
using Contracts.Products.Common;
using IntegrationTests.TestUtils.Contracts;

namespace IntegrationTests.Products.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductDiscountsRequest"/>
/// </summary>
public static class UpdateProductDiscountsRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductDiscountsRequest"/> class.
    /// </summary>
    /// <param name="discounts">The product discounts.</param>
    /// <returns>A new instance of the <see cref="UpdateProductDiscountsRequest"/> class.</returns>
    public static UpdateProductDiscountsRequest CreateRequest(IEnumerable<Discount>? discounts = null)
    {
        return new UpdateProductDiscountsRequest(discounts ?? [ContractDiscountUtils.CreateDiscount()]);
    }

}
