using Contracts.Products;

using Domain.ProductAggregate;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.Products;

/// <summary>
/// Extension methods for the <see cref="ProductResponse"/> response.
/// </summary>
public static class ProductResponseExtensions
{
    /// <summary>
    /// Ensures the response corresponds to the given product.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="product">The product to be tested against.</param>
    public static void EnsureCorrespondsTo(
        this ProductResponse response,
        Product product
    )
    {
        response.Id.Should().Be(product.Id.ToString());
        response.Name.Should().Be(product.Name);
        response.Description.Should().Be(product.Description);
        response.BasePrice.Should().Be(product.BasePrice);
        response.QuantityAvailable
            .Should()
            .Be(product.Inventory.QuantityAvailable);
        response.Images
            .Should()
            .BeEquivalentTo(product.ProductImages.Select(pi => pi.Uri));
    }

    /// <summary>
    /// Ensures a list of product responses corresponds to the given list of
    /// expected products.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="expectedProducts">The expected products.</param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<ProductResponse> response,
        IEnumerable<Product> expectedProducts
    )
    {
        foreach (var expected in expectedProducts)
        {
            var responseProduct = response
                .First(r => r.Id == expected.Id.ToString());

            responseProduct.EnsureCorrespondsTo(expected);
        }
    }

    /// <summary>
    /// Ensures the response was created from the specified request.
    /// </summary>
    /// <param name="response">The response object.</param>
    /// <param name="request">The request object.</param>
    public static void EnsureCreatedFromRequest(
        this ProductResponse response,
        CreateProductRequest request
    )
    {
        response.Id.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.BasePrice.Should().Be(request.BasePrice);
        response.Images.Should().BeEquivalentTo(request.Images);
    }
}
