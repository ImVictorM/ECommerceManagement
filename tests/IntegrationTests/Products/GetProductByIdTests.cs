using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Extensions.Http;

using Contracts.Products;

using WebApi.Products;

using Microsoft.AspNetCore.Routing;
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
    /// Verifies when the product with the specified identifier exists the response
    /// code is OK and the response content is correct.
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
    /// Verifies when the product with the specified identifier does not exist
    /// a not found response is returned.
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

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    /// <summary>
    /// Verifies when the product with the specified identifier is inactive
    /// a not found response is returned.
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

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
