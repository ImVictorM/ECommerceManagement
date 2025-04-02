using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Ensures that a product's sale price does not fall below a defined threshold.
/// This threshold is 10% of the product's base price,
/// meaning the maximum allowed discount is 90%.
/// </summary>
public class ProductSaleDiscountThresholdSpecification
    : CompositeSpecification<Product>
{
    private static readonly Percentage _thresholdPercentage = Percentage.Create(10);
    private readonly decimal _salePrice;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ProductSaleDiscountThresholdSpecification"/> class.
    /// </summary>
    /// <param name="salePrice">The sale price for the product.</param>
    public ProductSaleDiscountThresholdSpecification(decimal salePrice)
    {
        _salePrice = salePrice;
    }

    /// <inheritdoc/>
    public override bool IsSatisfiedBy(Product entity)
    {
        var minimumAllowedPrice =
            entity.BasePrice * (_thresholdPercentage.Value / 100m);

        return _salePrice >= minimumAllowedPrice;
    }
}
