using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.ProductAggregate;

/// <summary>
/// Represents a product aggregate.
/// </summary>
public sealed class Product : AggregateRoot<ProductId>, IActivatable
{
    private readonly List<ProductImage> _productImages = [];
    private readonly HashSet<ProductCategory> _productCategories = [];

    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    public string Description { get; private set; } = string.Empty;
    /// <inheritdoc/>
    public decimal BasePrice { get; private set; }
    /// <inheritdoc/>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the product inventory.
    /// </summary>
    public Inventory Inventory { get; private set; } = null!;
    /// <summary>
    /// Gets the product images.
    /// </summary>
    public IReadOnlyList<ProductImage> ProductImages => _productImages.AsReadOnly();
    /// <summary>
    /// Gets the product categories.
    /// </summary>
    public IReadOnlySet<ProductCategory> ProductCategories => _productCategories;

    private Product() { }

    private Product(
        string name,
        string description,
        decimal basePrice,
        Inventory inventory,
        IEnumerable<ProductCategory> productCategories,
        IEnumerable<ProductImage> productImages
    )
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;
        Inventory = inventory;

        _productCategories.UnionWith(productCategories);
        _productImages.AddRange(productImages);

        IsActive = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="initialQuantityInInventory">The initial quantity of this product in the inventory.</param>
    /// <param name="productCategories">The categories related to this product.</param>
    /// <param name="productImages">The product images.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product Create(
        string name,
        string description,
        decimal basePrice,
        int initialQuantityInInventory,
        IEnumerable<ProductCategory> productCategories,
        IEnumerable<ProductImage> productImages
    )
    {
        var inventory = Inventory.Create(initialQuantityInInventory);

        return new Product(
            name,
            description,
            basePrice,
            inventory,
            productCategories,
            productImages
        );
    }

    /// <summary>
    /// Updates the product base details.
    /// </summary>
    /// <param name="name">The new product name.</param>
    /// <param name="description">The new product description.</param>
    /// <param name="basePrice">The new product base price.</param>
    /// <param name="images">The new product images.</param>
    /// <param name="categories">The new product categories.</param>
    public void UpdateProduct(
        string name,
        string description,
        decimal basePrice,
        IEnumerable<ProductImage> images,
        IEnumerable<ProductCategory> categories
    )
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;

        _productImages.Clear();
        _productImages.AddRange(images);

        _productCategories.Clear();
        _productCategories.UnionWith(categories);
    }

    /// <summary>
    /// Increments the quantity of this product in inventory by the value specified.
    /// </summary>
    /// <param name="quantityToAdd">The quantity of products to add.</param>
    public void IncrementQuantityInInventory(int quantityToAdd)
    {
        Inventory.IncrementQuantityAvailable(quantityToAdd);
    }

    /// <inheritdoc/>
    public void Deactivate()
    {
        IsActive = false;
        Inventory.Reset();
    }
}
