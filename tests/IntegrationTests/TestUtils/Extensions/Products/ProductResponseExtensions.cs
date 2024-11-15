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
    /// Ensure a response was created based on a product.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="product">The product to be tested against.</param>
    public static void EnsureCreatedFrom(this ProductResponse response, Product product)
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
}
