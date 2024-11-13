using Domain.ProductAggregate;
using Domain.UnitTests.TestUtils.Constants;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// The product utilities.
/// </summary>
public static class ProductUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantityAvailable">The product quantity available in inventory.</param>
    /// <param name="categories">The product categories.</param>
    /// <param name="productImagesUrl">The product image urls.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product CreateProduct(
        string? name = null,
        string? description = null,
        decimal? price = null,
        int? quantityAvailable = null,
        IReadOnlyList<string>? categories = null,
        IReadOnlyList<Uri>? productImagesUrl = null
    )
    {
        return Product.Create(
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            price ?? DomainConstants.Product.Price,
            quantityAvailable ?? DomainConstants.Product.QuantityAvailable,
            categories ?? DomainConstants.Product.Categories,
            productImagesUrl ?? CreateProductImagesUrl()
        );
    }

    /// <summary>
    /// Creates a list of product image urls.
    /// </summary>
    /// <param name="imageCount">The quantity of product images to be created.</param>
    /// <returns>A list of product image urls.</returns>
    public static IReadOnlyList<Uri> CreateProductImagesUrl(int imageCount = 0)
    {
        return Enumerable
            .Range(0, imageCount)
            .Select(DomainConstants.Product.ProductImageFromIndex).ToList().AsReadOnly();
    }
}
