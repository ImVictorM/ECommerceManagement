using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a sale product.
/// </summary>
public class SaleProduct : ValueObject
{
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;
    /// <summary>
    /// Gets the product categories.
    /// </summary>
    public IReadOnlySet<CategoryId> Categories { get; } = null!;

    private SaleProduct() { }

    private SaleProduct(ProductId productId, IReadOnlySet<CategoryId> categories)
    {
        ProductId = productId;
        Categories = categories;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="categories">The product categories.</param>
    /// <returns>A new instance of the <see cref="SaleProduct"/> class.</returns>
    public static SaleProduct Create(ProductId productId, IReadOnlySet<CategoryId> categories)
    {
        return new SaleProduct(productId, categories);
    }

    ///<inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
        yield return Categories;
    }
}
