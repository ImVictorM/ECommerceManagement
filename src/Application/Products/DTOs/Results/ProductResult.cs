using Domain.ProductAggregate;

namespace Application.Products.DTOs.Results;

/// <summary>
/// Represents a product result.
/// </summary>
public class ProductResult
{
    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the product name.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets the product description.
    /// </summary>
    public string Description { get; }
    /// <summary>
    /// Gets the product base price.
    /// </summary>
    public decimal BasePrice { get; }
    /// <summary>
    /// Gets the product price with discount.
    /// </summary>
    public decimal PriceWithDiscount { get; }
    /// <summary>
    /// Gets the product quantity available in inventory.
    /// </summary>
    public int QuantityAvailable { get; }
    /// <summary>
    /// Gets the product category names.
    /// </summary>
    public IReadOnlyList<string> CategoryIds { get; }
    /// <summary>
    /// Gets the product image URIs.
    /// </summary>
    public IReadOnlyList<Uri> Images { get; }

    private ProductResult(
        Product product,
        decimal discountedPrice
    )
    {
        Id = product.Id.ToString();
        Name = product.Name;
        Description = product.Description;
        BasePrice = product.BasePrice;
        PriceWithDiscount = discountedPrice;
        QuantityAvailable = product.Inventory.QuantityAvailable;

        CategoryIds = product.ProductCategories
            .Select(category => category.CategoryId.ToString())
            .ToList();

        Images = product.ProductImages
            .Select(image => image.Uri)
            .ToList();
    }

    internal static ProductResult FromProductWithDiscountedPrice(
        Product product,
        decimal discountedPrice
    )
    {
        return new ProductResult(product, discountedPrice);
    }
}
