using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.Enumerations;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Errors;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.Services;
using SharedKernel.Specifications;
using SharedKernel.ValueObjects;

namespace Domain.ProductAggregate;

/// <summary>
/// Represents a product aggregate.
/// </summary>
public sealed class Product : AggregateRoot<ProductId>, ISoftDeletable, IDiscountable
{
    private readonly List<ProductImage> _productImages = [];
    private readonly List<Discount> _discounts = [];
    private readonly List<ProductCategory> _productCategories = [];

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
    /// <inheritdoc/>
    public IReadOnlyList<Discount> Discounts => _discounts.AsReadOnly();
    /// <summary>
    /// Gets the product categories.
    /// </summary>
    public IReadOnlyList<ProductCategory> ProductCategories => _productCategories.AsReadOnly();

    private Product() { }

    private Product(
        string name,
        string description,
        decimal basePrice,
        Inventory inventory,
        IEnumerable<ProductCategory> productCategories,
        IEnumerable<ProductImage> productImages,
        IEnumerable<Discount>? initialDiscounts = null
    )
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;
        Inventory = inventory;

        _productCategories.AddRange(productCategories);
        _productImages.AddRange(productImages);

        if (initialDiscounts != null)
        {
            AddDiscounts(initialDiscounts.ToArray());
        }

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
    /// <param name="productImageUrls">The product images.</param>
    /// <param name="initialDiscounts">The initial discount the product will have.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product Create(
        string name,
        string description,
        decimal basePrice,
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
            basePrice,
            inventory,
            categories,
            images,
            initialDiscounts
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
        IEnumerable<Uri> images,
        IEnumerable<string> categories
    )
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;

        _productImages.Clear();
        _productImages.AddRange(images.Select(ProductImage.Create));

        _productCategories.Clear();
        _productCategories.AddRange(
            categories
                .Select(Category.Create)
                .Select(ProductCategory.Create)
        );
    }

    /// <summary>
    /// Gets the product category names.
    /// </summary>
    /// <returns>The product category names.</returns>
    public IEnumerable<string> GetCategoryNames()
    {
        return ProductCategories.Select(pc => Category.Create(pc.CategoryId).Name);
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
    public void AddDiscounts(params Discount[] discounts)
    {
        _discounts.AddRange(discounts);

        if (!new DiscountThresholdSpecification().IsSatisfiedBy(this))
        {
            throw new DomainValidationException("Total discounts exceed the allowed threshold");
        }
    }

    /// <inheritdoc/>
    public decimal GetPriceAfterDiscounts()
    {
        return DiscountService.ApplyDiscounts(BasePrice, [.. Discounts]);
    }

    /// <inheritdoc/>
    public void MakeInactive()
    {
        IsActive = false;
        Inventory.Reset();
    }
}
