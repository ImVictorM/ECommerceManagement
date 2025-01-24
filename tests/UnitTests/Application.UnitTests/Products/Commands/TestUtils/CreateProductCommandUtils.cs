using Application.Products.Commands.CreateProduct;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utility class for creating instances of <see cref="CreateProductCommand"/> with customizable parameters for testing purposes.
/// Provides default values based on predefined domain constants.
/// </summary>
public static class CreateProductCommandUtils
{
    private static readonly Faker _faker = new();
    /// <summary>
    /// Creates a new instance of the <see cref="CreateProductCommand"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="initialQuantity">The product initial quantity.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="categoryIds">The product categories.</param>
    /// <param name="images">The product images.</param>
    /// <returns>A new instance of the <see cref="CreateProductCommand"/> class.</returns>
    public static CreateProductCommand CreateCommand(
        string? name = null,
        string? description = null,
        int? initialQuantity = null,
        decimal? basePrice = null,
        IEnumerable<string>? categoryIds = null,
        IEnumerable<Uri>? images = null
    )
    {
        return new CreateProductCommand(
            name ?? _faker.Commerce.ProductName(),
            description ?? _faker.Commerce.ProductDescription(),
            initialQuantity ?? _faker.Random.Int(1, 100),
            basePrice ?? _faker.Random.Decimal(10m, 2000m),
            categoryIds ?? NumberUtils.CreateNumberSequenceAsString(2),
            images ?? ProductUtils.CreateImageURIs()
        );
    }
}
