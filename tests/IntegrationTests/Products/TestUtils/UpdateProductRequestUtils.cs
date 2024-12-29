using Contracts.Products;
using Domain.UnitTests.TestUtils.Constants;

namespace IntegrationTests.Products.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductRequest"/> request.
/// </summary>
public static class UpdateProductRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductRequest"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="images">The product images.</param>
    /// <param name="categoryIds">The product categories.</param>
    /// <returns>A new instance of the <see cref="UpdateProductRequest"/> class.</returns>
    public static UpdateProductRequest CreateRequest(
        string? name = null,
        string? description = null,
        decimal? basePrice = null,
        IEnumerable<Uri>? images = null,
        IEnumerable<string>? categoryIds = null
    )
    {
        return new UpdateProductRequest(
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            basePrice ?? DomainConstants.Product.BasePrice,
            images ?? DomainConstants.Product.ProductImages.Select(i => i.Url),
            categoryIds ?? DomainConstants.Product.Categories.Select(c => c.CategoryId.ToString())
        );
    }
}
