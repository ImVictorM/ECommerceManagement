using System.Globalization;
using Application.Products.Commands.UpdateProductDiscounts;
using Domain.UnitTests.TestUtils.Constants;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductDiscountsCommand"/>
/// </summary>
public static class UpdateProductDiscountsCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductDiscountsCommand"/> class.
    /// </summary>
    /// <param name="id">The product id.</param>
    /// <param name="discounts">The product discounts.</param>
    /// <returns>A new instance of the <see cref="UpdateProductDiscountsCommand"/> class.</returns>
    public static UpdateProductDiscountsCommand CreateCommand(
        string? id = null,
        IEnumerable<Discount>? discounts = null
    )
    {
        return new UpdateProductDiscountsCommand(
            id ?? DomainConstants.Product.Id.ToString(CultureInfo.InvariantCulture),
            discounts ?? [DiscountUtils.CreateDiscount(percentage: 10)]
       );
    }
}
