using Domain.Common.Models;
using Domain.ProductCategoryAggregate.ValueObjects;

namespace Domain.ProductCategoryAggregate;

/// <summary>
/// Represents a product category.
/// </summary>
public sealed class ProductCategory : AggregateRoot<ProductCategoryId>
{
    /// <summary>
    /// Gets the name of the product category.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategory"/> class.
    /// </summary>
    private ProductCategory() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategory"/> class.
    /// </summary>
    /// <param name="name">The name of the product category.</param>
    private ProductCategory(string name)
        : base(ProductCategoryId.Create())
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductCategory"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the product category.</param>
    /// <returns>A new instance of <see cref="ProductCategory"/> with the specified name.</returns>
    public static ProductCategory Create(string name)
    {
        return new ProductCategory(name);
    }
}
