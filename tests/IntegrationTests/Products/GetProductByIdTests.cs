using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Extensions.Http;

using Contracts.Products;

using WebApi.Products;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Xunit.Abstractions;
using System.Net;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the get product by id feature.
/// </summary>
public class GetProductByIdTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetProductByIdTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies when the product with the specified id exists the response code
    /// is OK and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsOk()
    {
        var productToFetch = _seedProduct.GetEntity(
            ProductSeedType.COMPUTER_ON_SALE
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetProductById),
            new { id = productToFetch.Id.ToString() }
        );

        var response = await _client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProductResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        responseContent.EnsureCorrespondsTo(productToFetch);
    }

    /// <summary>
    /// Verifies when the product with the specified id does not exist the response
    /// code is NOT_FOUND and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductDoesNotExists_ReturnsNotFound()
    {
        var invalidProductId = "9999";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetProductById),
            new { id = invalidProductId }
        );

        var response = await _client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be(
            $"The product with id {invalidProductId} does not exist"
        );
    }
    /// <summary>
    /// Verifies when the product with the specified id is inactive the response
    /// code is NOT_FOUND and the response content is correct.
    /// </summary>
    [Fact]
    public async Task GetProductById_WhenProductIsInactive_ReturnsNotFound()
    {
        var idProductInactive = _seedProduct
            .GetEntityId(ProductSeedType.JACKET_INACTIVE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetProductById),
            new { id = idProductInactive }
        );

        var response = await _client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be(
            $"The product with id {idProductInactive} does not exist"
        );
    }
}
