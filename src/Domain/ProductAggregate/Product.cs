using Domain.Common.Models;
using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.ValueObjects;
using Domain.ProductCategoryAggregate.ValueObjects;

namespace Domain.ProductAggregate;

/// <summary>
/// Represents a product aggregate.
/// </summary>
public sealed class Product : AggregateRoot<ProductId>
{
    /// <summary>
    /// The product images.
    /// </summary>
    private readonly List<ProductImage> _productImages = [];
    /// <summary>
    /// The product discounts.
    /// </summary>
    private readonly List<ProductDiscount>? _productDiscounts = [];
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    public string Description { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public float Price { get; private set; }
    /// <summary>
    /// A boolean value indicating if the product is active.
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the product inventory.
    /// </summary>
    public Inventory Inventory { get; private set; } = null!;
    /// <summary>
    /// Gets the product categories.
    /// </summary>
    public ProductCategoryId ProductCategoryId { get; private set; } = null!;
    /// <summary>
    /// Gets the product images.
    /// </summary>
    public IReadOnlyList<ProductImage> ProductImages => _productImages.AsReadOnly();
    /// <summary>
    /// Gets the product discount that holds a list of discounts.
    /// </summary>
    public IReadOnlyList<ProductDiscount>? ProductDiscounts => _productDiscounts?.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    private Product() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="inventory">The product inventory.</param>
    private Product(
        string name,
        string description,
        float price,
        Inventory inventory
    ) : base(ProductId.Create())
    {
        Name = name;
        Description = description;
        Price = price;
        Inventory = inventory;
        IsActive = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="inventory">The product inventory.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product Create(
        string name,
        string description,
        float price,
        Inventory inventory
    )
    {
        return new Product(
            name,
            description,
            price,
            inventory
        );
    }
}