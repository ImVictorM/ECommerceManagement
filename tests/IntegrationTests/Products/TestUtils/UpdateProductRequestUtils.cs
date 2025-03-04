using Contracts.Products;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace IntegrationTests.Products.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductRequest"/> request.
/// </summary>
public static class UpdateProductRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductRequest"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="images">The product images.</param>
    /// <param name="categoryIds">The product categories.</param>
    /// <returns>
    /// A new instance of the <see cref="UpdateProductRequest"/> class.
    /// </returns>
    public static UpdateProductRequest CreateRequest(
        string? name = null,
        string? description = null,
        decimal? basePrice = null,
        IEnumerable<Uri>? images = null,
        IEnumerable<string>? categoryIds = null
    )
    {

        return new UpdateProductRequest(
            name ?? _faker.Commerce.ProductName(),
            description ?? _faker.Commerce.ProductDescription(),
            basePrice ?? _faker.Random.Decimal(10m, 2500m),
            images ?? ProductUtils.CreateImageURIs(3),
            categoryIds ?? NumberUtils.CreateNumberSequenceAsString(2)
        );
    }
}
