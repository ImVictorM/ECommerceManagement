using System.Net.Http.Json;
using Contracts.Products;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests covering the process of getting a product by id.
/// </summary>
public class GetProductByIdTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetProductByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests when the product with the specified id exists the response code is OK and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductExists_RetrievesItAndReturnsOk()
    {
        var productToFetch = ProductSeed.GetSeedProduct(SeedAvailableProducts.COMPUTER_WITH_DISCOUNTS);

        var response = await Client.GetAsync($"/products/{productToFetch.Id}");
        var responseContent = await response.Content.ReadFromJsonAsync<ProductResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        responseContent!.EnsureCorrespondsTo(productToFetch);
    }

    /// <summary>
    /// Tests when the product with the specified id does not exist the response code is NOT_FOUND and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductDoesNotExists_ReturnsNotFound()
    {
        var invalidId = "4000";
        var response = await Client.GetAsync($"/products/{invalidId}");
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)System.Net.HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"The product with id {invalidId} does not exist");
    }
}
