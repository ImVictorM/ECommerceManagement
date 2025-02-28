using Contracts.Products;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace IntegrationTests.Products.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateProductRequest"/> class.
/// </summary>
public static class CreateProductRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="CreateProductRequest"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="initialQuantity">The product initial quantity.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="categoryIds">The product categories.</param>
    /// <param name="images">The product images.</param>
    /// <returns>
    /// A new instance of the <see cref="CreateProductRequest"/> class.
    /// </returns>
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
            name ?? _faker.Commerce.ProductName(),
            description ?? _faker.Commerce.ProductDescription(),
            initialQuantity ?? _faker.Random.Int(1, 100),
            basePrice ?? _faker.Random.Decimal(10m, 2500m),
            categoryIds ?? NumberUtils.CreateNumberSequenceAsString(2),
            images ?? ProductUtils.CreateImageURIs(3)
        );
    }
}
