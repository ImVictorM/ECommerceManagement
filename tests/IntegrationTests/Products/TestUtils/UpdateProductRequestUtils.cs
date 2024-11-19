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
    /// <param name="categories">The product categories.</param>
    /// <returns>A new instance of the <see cref="UpdateProductRequest"/> class.</returns>
    public static UpdateProductRequest CreateRequest(
        string? name = null,
        string? description = null,
        decimal? basePrice = null,
        IEnumerable<Uri>? images = null,
        IEnumerable<string>? categories = null
    )
    {
        return new UpdateProductRequest(
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            basePrice ?? DomainConstants.Product.Price,
            images ?? [DomainConstants.Product.ProductImage],
            categories ?? DomainConstants.Product.Categories
        );
    }
}
