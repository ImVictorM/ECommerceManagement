using Application.Products.Commands.CreateProduct;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utility class for creating instances of <see cref="CreateProductCommand"/> with customizable parameters for testing purposes.
/// Provides default values based on predefined domain constants.
/// </summary>
public static class CreateProductCommandUtils
{

    /// <summary>
    /// Creates a new instance of the <see cref="CreateProductCommand"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="initialQuantity">The product initial quantity.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="categories">The product categories.</param>
    /// <param name="images">The product images.</param>
    /// <returns>A new instance of the <see cref="CreateProductCommand"/> class.</returns>
    public static CreateProductCommand CreateCommand(
        string? name = null,
        string? description = null,
        int? initialQuantity = null,
        decimal? basePrice = null,
        IEnumerable<long>? categories = null,
        IEnumerable<Uri>? images = null
    )
    {
        return new CreateProductCommand(
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            initialQuantity ?? DomainConstants.Product.QuantityInInventory,
            basePrice ?? DomainConstants.Product.BasePrice,
            categories ?? DomainConstants.Product.Categories.Select(c => c.CategoryId.Value),
            images ?? DomainConstants.Product.ProductImages.Select(i => i.Url)
        );
    }
}
