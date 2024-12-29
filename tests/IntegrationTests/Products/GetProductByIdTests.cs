using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Seeds;

using Contracts.Products;

using System.Net.Http.Json;
using FluentAssertions;
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
    /// List of ids that correspond to inactive or products that does not exist.
    /// </summary>
    public static readonly IEnumerable<object[]> NotFoundProductIds =
    [
        [ProductSeed.GetSeedProduct(SeedAvailableProducts.INACTIVE_JACKET).Id],
        [404],
    ];

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
    /// Tests when the product with the specified id does not exist or is inactive the response code is NOT_FOUND and the response content is correct.
    /// </summary>
    [Theory]
    [MemberData(nameof(NotFoundProductIds))]
    public async Task GetProductById_WhenProductDoesNotExistsOrIsInactive_ReturnsNotFound(long notFoundId)
    {
        var response = await Client.GetAsync($"/products/{notFoundId}");
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)System.Net.HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"The product with id {notFoundId} does not exist");
    }
}
