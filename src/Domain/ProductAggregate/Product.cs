using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.Enumerations;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.ProductAggregate;

/// <summary>
/// Represents a product aggregate.
/// </summary>
public sealed class Product : AggregateRoot<ProductId>, ISoftDeletable
{
    private readonly List<ProductImage> _productImages = [];
    private readonly List<Discount> _productDiscounts = [];
    private readonly List<ProductCategory> _productCategories = [];

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
    /// Gets the product images.
    /// </summary>
    public IReadOnlyList<ProductImage> ProductImages => _productImages.AsReadOnly();
    /// <summary>
    /// Gets the product discount that holds a list of discounts.
    /// </summary>
    public IReadOnlyList<Discount> ProductDiscounts => _productDiscounts.AsReadOnly();
    /// <summary>
    /// Gets the product categories.
    /// </summary>
    public IReadOnlyList<ProductCategory> ProductCategories => _productCategories.AsReadOnly();

    private Product() { }

    private Product(
        string name,
        string description,
        decimal price,
        Inventory inventory,
        IEnumerable<ProductCategory> productCategories,
        IEnumerable<ProductImage> productImages,
        IEnumerable<Discount>? initialDiscounts = null
    )
    {
        Name = name;
        Description = description;
        Price = price;
        Inventory = inventory;

        _productCategories.AddRange(productCategories);
        _productImages.AddRange(productImages);

        if (initialDiscounts != null)
        {
            _productDiscounts.AddRange(initialDiscounts);
        }

        IsActive = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>

    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="initialQuantityInInventory">The initial quantity of this product in the inventory.</param>
    /// <param name="productCategories">The categories related to this product.</param>
    /// <param name="productImageUrls">The product images.</param>
    /// <param name="initialDiscounts">The initial discount the product will have.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product Create(
        string name,
        string description,
        decimal price,
        int initialQuantityInInventory,
        IEnumerable<string> productCategories,
        IEnumerable<Uri> productImageUrls,
        IEnumerable<Discount>? initialDiscounts = null
    )
    {
        var inventory = Inventory.Create(initialQuantityInInventory);

        var images = productImageUrls.Select(ProductImage.Create);

        var categories = productCategories
            .Select(Category.Create)
            .Select(ProductCategory.Create);

        return new Product(
            name,
            description,
            price,
            inventory,
            categories,
            images,
            initialDiscounts
        );
    }

    /// <summary>
    /// Adds a new discount to the product.
    /// </summary>
    /// <param name="discount">The discount.</param>
    public void AddDiscount(Discount discount)
    {
        _productDiscounts.Add(discount);
    }

    /// <inheritdoc/>
    public void MakeInactive()
    {
        IsActive = false;
    }
}
