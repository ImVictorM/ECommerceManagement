using Domain.Common.Models;
using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.ValueObjects;

namespace Domain.ProductAggregate;

/// <summary>
/// Represents a product aggregate.
/// </summary>
public sealed class Product : AggregateRoot<ProductId>
{
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    public string Description { get; private set; }
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
    public Inventory Inventory { get; private set; }
    /// <summary>
    /// Gets the product images.
    /// </summary>
    public IEnumerable<ProductImage> Images { get; private set; }
    /// <summary>
    /// Gets the product categories.
    /// </summary>
    public IEnumerable<ProductCategory> Categories { get; private set; }
    /// <summary>
    /// Gets the product discount that holds a list of discounts.
    /// </summary>
    public IEnumerable<ProductDiscount>? ProductDiscounts { get; private set; }

    /// <summary>
    /// Gets the product feedback ids.
    /// </summary>
    public IEnumerable<ProductFeedbackId>? ProductFeedbackIds { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="inventory">The product inventory.</param>
    /// <param name="images">The product images.</param>
    /// <param name="categories">The product categories.</param>
    private Product(
        string name,
        string description,
        float price,
        Inventory inventory,
        IEnumerable<ProductImage> images,
        IEnumerable<ProductCategory> categories,
        IEnumerable<ProductDiscount>? productDiscounts,
        IEnumerable<ProductFeedbackId> productFeedbackIds
    ) : base(ProductId.Create())
    {
        Name = name;
        Description = description;
        Price = price;
        Inventory = inventory;
        Images = images;
        Categories = categories;
        ProductDiscounts = productDiscounts;
        ProductFeedbackIds = productFeedbackIds;
        IsActive = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="inventory">The product inventory.</param>
    /// <param name="images">The product images.</param>
    /// <param name="categories">The product categories.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product Create(
        string name,
        string description,
        float price,
        Inventory inventory,
        IEnumerable<ProductImage> images,
        IEnumerable<ProductDiscount>? productDiscounts,
        IEnumerable<ProductCategory> categories,
        IEnumerable<ProductFeedbackId> productFeedbackIds
    )
    {
        return new Product(
            name,
            description,
            price,
            inventory,
            images,
            categories,
            productDiscounts,
            productFeedbackIds
        );
    }
}
