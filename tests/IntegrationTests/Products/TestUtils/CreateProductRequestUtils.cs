using Contracts.Products;
using Domain.UnitTests.TestUtils.Constants;

namespace IntegrationTests.Products.TestUtils;

/// <summary>
/// Utility class for creating instances of <see cref="CreateProductRequest"/> with default or customized values.
/// Provides methods to simplify test setup for product creation requests by allowing specific fields to be overridden.
/// </summary>
public static class CreateProductRequestUtils
{

    /// <summary>
    /// Creates a new instance of the <see cref="CreateProductRequest"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="initialQuantity">The product initial quantity.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="categoryIds">The product categories.</param>
    /// <param name="images">The product images.</param>
    /// <returns>A new instance of the <see cref="CreateProductRequest"/> class.</returns>
    public static CreateProductRequest CreateRequest(
        string? name = null,
        string? description = null,
        int? initialQuantity = null,
        decimal? basePrice = null,
        IEnumerable<string>? categoryIds = null,
        IEnumerable<Uri>? images = null
    )
    {
        return new CreateProductRequest(
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            initialQuantity ?? DomainConstants.Product.QuantityInInventory,
            basePrice ?? DomainConstants.Product.BasePrice,
            categoryIds ?? DomainConstants.Product.Categories.Select(c => c.CategoryId.ToString()),
            images ?? DomainConstants.Product.ProductImages.Select(i => i.Url)
        );
    }
}
