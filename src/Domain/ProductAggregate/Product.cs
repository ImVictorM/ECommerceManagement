using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Common.ValueObjects;
using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate;

/// <summary>
/// Represents a product aggregate.
/// </summary>
public sealed class Product : AggregateRoot<ProductId>, ISoftDeletable
{
    /// <summary>
    /// The product images.
    /// </summary>
    private readonly List<ProductImage> _productImages = [];
    /// <summary>
    /// The product discounts.
    /// </summary>
    private readonly List<ProductDiscount> _productDiscounts = [];
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
    public decimal Price { get; private set; }
    /// <summary>
    /// A boolean value indicating if the product is active.
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the product inventory.
    /// </summary>
    public Inventory Inventory { get; private set; } = null!;
    /// <summary>
    /// Gets the product category.
    /// </summary>
    public ProductCategory ProductCategory { get; private set; } = null!;
    /// <summary>
    /// Gets the product images.
    /// </summary>
    public IReadOnlyList<ProductImage> ProductImages => _productImages.AsReadOnly();
    /// <summary>
    /// Gets the product discount that holds a list of discounts.
    /// </summary>
    public IReadOnlyList<ProductDiscount> ProductDiscounts => _productDiscounts.AsReadOnly();

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
    /// <param name="productCategory">The category related to this product.</param>
    /// <param name="productImages">The product images.</param>
    private Product(
        string name,
        string description,
        decimal price,
        Inventory inventory,
        ProductCategory productCategory,
        List<ProductImage> productImages
    )
    {
        Name = name;
        Description = description;
        Price = price;
        Inventory = inventory;
        ProductCategory = productCategory;

        _productImages.AddRange(productImages);

        IsActive = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="category">The category related to this product.</param>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantityAvailable">The quantity of this product in inventory.</param>
    /// <param name="productImageUrls">The product images.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product Create(
        string name,
        string description,
        decimal price,
        int quantityAvailable,
        ProductCategory category,
        IEnumerable<Uri> productImageUrls
    )
    {
        var inventory = Inventory.Create(quantityAvailable);
        var productImages = productImageUrls.Select(ProductImage.Create).ToList();

        return new Product(
            name,
            description,
            price,
            inventory,
            category,
            productImages
        );
    }

    /// <summary>
    /// Adds a new discount to the product by id.
    /// </summary>
    /// <param name="discount">The discount.</param>
    public void AddDiscount(Discount discount)
    {
        _productDiscounts.Add(ProductDiscount.Create(discount));
    }

    /// <inheritdoc/>
    public void MakeInactive()
    {
        IsActive = false;
    }
}
