using Domain.ProductAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Extensions.Http;

using Contracts.Products;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests covering the process of getting a product by id.
/// </summary>
public class GetProductByIdTests : BaseIntegrationTest
{
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetProductByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
    }

    /// <summary>
    /// Tests when the product with the specified id exists the response code is OK and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductExists_RetrievesItAndReturnsOk()
    {
        var productToFetch = _seedProduct.GetByType(ProductSeedType.COMPUTER_ON_SALE);

        var response = await RequestService.Client.GetAsync($"/products/{productToFetch.Id}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<ProductResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        responseContent.EnsureCorrespondsTo(productToFetch);
    }

    /// <summary>
    /// Tests when the product with the specified id does not exist the response code is NOT_FOUND and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductDoesNotExists_ReturnsNotFound()
    {
        var invalidProductId = 9999;

        var response = await RequestService.Client.GetAsync($"/products/{invalidProductId}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)System.Net.HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"The product with id {invalidProductId} does not exist");
    }
    /// <summary>
    /// Tests when the product with the specified id is inactive the response code is NOT_FOUND and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductIsInactive_ReturnsNotFound()
    {
        var productInactive = _seedProduct.GetByType(ProductSeedType.JACKET_INACTIVE);

        var response = await RequestService.Client.GetAsync($"/products/{productInactive.Id}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)System.Net.HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"The product with id {productInactive.Id} does not exist");
    }
}
