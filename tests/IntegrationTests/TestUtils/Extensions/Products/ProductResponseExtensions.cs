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
    /// Ensures a response corresponds to a product.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="product">The product to be tested against.</param>
    public static void EnsureCorrespondsTo(this ProductResponse response, Product product)
    {
        response!.Id.Should().Be(product.Id.ToString());
        response.Name.Should().Be(product.Name);
        response.QuantityAvailable.Should().Be(product.Inventory.QuantityAvailable);
        response.Categories.Should().BeEquivalentTo(product.GetCategoryNames());
        response.Description.Should().Be(product.Description);
        response.Images.Should().BeEquivalentTo(product.ProductImages.Select(pi => pi.Url));
        response.OriginalPrice.Should().Be(product.BasePrice);
        response.PriceWithDiscount.Should().Be(product.GetPriceAfterDiscounts());
    }

    /// <summary>
    /// Ensures a list of product responses corresponds to a list of expected products.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="expectedProducts">The expected products.</param>
    public static void EnsureCorrespondsTo(this IEnumerable<ProductResponse> response, IEnumerable<Product> expectedProducts)
    {
        foreach (var expected in expectedProducts)
        {
            var responseProduct = response.First(r => r.Id == expected.Id.ToString());

            responseProduct.EnsureCorrespondsTo(expected);
        }
    }
}
