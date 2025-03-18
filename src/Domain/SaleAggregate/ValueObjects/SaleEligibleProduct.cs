using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a product along with its associated categories 
/// used for determining its eligibility for a sale.
/// </summary>
public class SaleEligibleProduct : ValueObject
{
    private readonly HashSet<CategoryId> _categoryIds = [];

    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;
    /// <summary>
    /// Gets the collection of category ids associated with the product.
    /// </summary>
    public IReadOnlySet<CategoryId> CategoryIds => _categoryIds;

    private SaleEligibleProduct() { }

    private SaleEligibleProduct(
        ProductId productId,
        IEnumerable<CategoryId> categoryIds
    )
    {
        ProductId = productId;
        _categoryIds = categoryIds.ToHashSet();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleEligibleProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="categoryIds">The product categories.</param>
    /// <returns>
    /// A new instance of the <see cref="SaleEligibleProduct"/> class.
    /// </returns>
    public static SaleEligibleProduct Create(
        ProductId productId,
        IEnumerable<CategoryId> categoryIds
    )
    {
        return new SaleEligibleProduct(productId, categoryIds);
    }

    ///<inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;

        foreach (var category in _categoryIds)
        {
            yield return category;
        }
    }
}
