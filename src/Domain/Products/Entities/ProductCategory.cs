using Domain.Common.Models;
using Domain.Products.ValueObjects;

namespace Domain.Products.Entities;

/// <summary>
/// Represents a product category.
/// </summary>
public sealed class ProductCategory : Entity<ProductCategoryId>
{
    /// <summary>
    /// Gets the name of the product category.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategory"/> class with the specified identifier and name.
    /// </summary>
    /// <param name="id">The unique identifier for this product category.</param>
    /// <param name="name">The name of the product category.</param>
    private ProductCategory(
        ProductCategoryId id,
        string name
    )
        : base(id)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductCategory"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the product category.</param>
    /// <returns>A new instance of <see cref="ProductCategory"/> withe the specified name.</returns>
    public static ProductCategory Create(string name)
    {
        return new ProductCategory(ProductCategoryId.Create(), name);
    }
}
