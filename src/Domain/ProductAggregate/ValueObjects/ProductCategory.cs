using Domain.CategoryAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents a product category.
/// </summary>
public class ProductCategory : ValueObject
{
    /// <summary>
    /// Gets the category identifier.
    /// </summary>
    public CategoryId CategoryId { get; } = null!;

    private ProductCategory()
    {
    }

    private ProductCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductCategory"/> class.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductCategory"/> class.
    /// </returns>
    public static ProductCategory Create(CategoryId categoryId)
    {
        return new ProductCategory(categoryId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoryId;
    }
}
