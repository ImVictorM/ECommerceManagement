using Domain.ProductAggregate.Enumerations;
using SharedKernel.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents a product category.
/// Used mainly to create a many-to-many relationship between products and categories.
/// </summary>
public class ProductCategory : ValueObject
{
    private readonly long _categoryId;

    /// <summary>
    /// Gets the category.
    /// </summary>
    public Category Category { get; } = null!;

    private ProductCategory() { }

    private ProductCategory(Category category)
    {
        _categoryId = category.Id;
        Category = category;
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
        yield return _categoryId;
    }
}
