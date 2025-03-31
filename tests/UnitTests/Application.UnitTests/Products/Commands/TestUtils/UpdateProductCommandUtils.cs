using Application.Products.Commands.UpdateProduct;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateProductCommand"/> command.
/// </summary>
public static class UpdateProductCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="UpdateProductCommand"/> class.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="images">The product images.</param>
    /// <param name="categoryIds">The product category identifiers.</param>
    /// <returns>
    /// A new instance of the <see cref="UpdateProductCommand"/> class.
    /// </returns>
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
            id ?? NumberUtils.CreateRandomLongAsString(),
            name ?? _faker.Commerce.ProductName(),
            description ?? _faker.Commerce.ProductDescription(),
            basePrice ?? _faker.Random.Decimal(10m, 1000m),
            images ?? ProductUtils.CreateImageURIs(2),
            categoryIds ?? NumberUtils.CreateNumberSequenceAsString(2)
        );
    }
}
