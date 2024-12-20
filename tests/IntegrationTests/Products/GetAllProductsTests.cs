using System.Net.Http.Json;
using Contracts.Products;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Seeds;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for getting all the products.
/// </summary>
public class GetAllProductsTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetAllProductsTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests that is possible to get all the products that are active.
    /// </summary>
    [Fact]
    public async Task GetAllProducts_WhenGettingAllProducts_ReturnsOkContainingTheActiveProducts()
    {
        var activeProduct = ProductSeed.ListProducts(product => product.IsActive);

        var response = await Client.GetAsync("/products");
        var responseContent = await response.Content.ReadFromJsonAsync<ProductListResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent!.Products.EnsureCorrespondsTo(activeProduct);
    }

    /// <summary>
    /// Tests that is possible to get the products with a limit.
    /// </summary>
    [Fact]
    public async Task GetAllProducts_WhenGettingProductsWithLimitParameter_ReturnsOkContainingProductsSubset()
    {
        var limit = 2;
        var response = await Client.GetAsync($"/products?limit={limit}");
        var responseContent = await response.Content.ReadFromJsonAsync<ProductListResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent!.Products.Count().Should().Be(limit);
    }
}
