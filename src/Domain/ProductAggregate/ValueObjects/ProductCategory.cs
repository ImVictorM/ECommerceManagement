using Domain.ProductAggregate.Enumerations;
using SharedKernel.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents a product category.
/// The purpose of this class is to create a many-to-many relationship between products and categories.
/// </summary>
public class ProductCategory : ValueObject
{
    /// <summary>
    /// Gets the category id.
    /// </summary>
    public long CategoryId { get; }

    private ProductCategory()
    {
    }

    private ProductCategory(Category category)
    {
        CategoryId = category.Id;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductCategory"/> class.
    /// </summary>
    /// <param name="category">The category of the product.</param>
    /// <returns>A new instance of the <see cref="ProductCategory"/> class.</returns>
    public static ProductCategory Create(Category category)
    {
        return new ProductCategory(category);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoryId;
    }
}
