using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Products;

using WebApi.Products;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the get products feature.
/// </summary>
public class GetProductsTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;
    private readonly ICategorySeed _seedCategory;
    private readonly string? _endpoint;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetProductsTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
        _seedCategory = SeedManager.GetSeed<ICategorySeed>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetProducts)
        );

        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies an OK response is returned containing all the active products
    /// by default.
    /// </summary>
    [Fact]
    public async Task GetProducts_WithValidRequest_ReturnsOk()
    {
        var activeProduct = _seedProduct.ListAll(product => product.IsActive);

        var response = await _client.GetAsync(_endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(activeProduct);
    }

    /// <summary>
    /// Verifies an OK response is returned containing the paginated products subset.
    /// </summary>
    [Fact]
    public async Task GetProducts_WithPagination_ReturnsOk()
    {
        var page = 1;
        var pageSize = 2;

        var response = await _client.GetAsync(
            $"{_endpoint}?page={page}&pageSize={pageSize}"
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Count().Should().Be(pageSize);
    }

    /// <summary>
    /// Verifies an OK response is returned containing the filtered products by
    /// category.
    /// </summary>
    [Fact]
    public async Task GetProducts_WithCategoryFilter_ReturnsOk()
    {
        var fashionCategoryId = _seedCategory
            .GetEntityId(CategorySeedType.FASHION)
            .ToString();

        var response = await _client.GetAsync(
            $"{_endpoint}?category={fashionCategoryId}"
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        foreach (var product in responseContent)
        {
            product.CategoryIds.Should().Contain(fashionCategoryId);
        }
    }
}
