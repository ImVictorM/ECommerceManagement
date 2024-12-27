using Application.Products.Commands.UpdateProduct;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductCommand"/> command.
/// </summary>
public static class UpdateProductCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductCommand"/> class.
    /// </summary>
    /// <param name="id">The product id.</param>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="images">The product images.</param>
    /// <param name="categoryIds">The product category ids.</param>
    /// <returns>A new instance of the <see cref="UpdateProductCommand"/> class.</returns>
    public static UpdateProductCommand CreateCommand(
        string? id = null,
        string? name = null,
        string? description = null,
        decimal? basePrice = null,
        IEnumerable<Uri>? images = null,
        IEnumerable<string>? categoryIds = null
    )
    {
        return new UpdateProductCommand(
            id ?? DomainConstants.Product.Id.ToString(),
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            basePrice ?? DomainConstants.Product.BasePrice,
            images ?? DomainConstants.Product.ProductImages.Select(i => i.Url),
            categoryIds ?? DomainConstants.Product.Categories.Select(c => c.CategoryId.ToString())
        );
    }
}
