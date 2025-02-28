using Domain.CategoryAggregate;
using Domain.ProductAggregate;

using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Products;

using WebApi.Products;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the get all products feature.
/// </summary>
public class GetAllProductsTests : BaseIntegrationTest
{
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;
    private readonly string? _endpoint;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllProductsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetAllProductsTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
        _endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetAllProducts)
        );
        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies an OK response is returned containing all the active products
    /// by default.
    /// </summary>
    [Fact]
    public async Task GetAllProducts_WithValidRequest_ReturnsOk()
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
    public async Task GetAllProducts_WithPagination_ReturnsOk()
    {
        var page = 1;
        var pageSize = 2;

        var response = await _client.GetAsync(
            $"{_endpoint}?page={page}&pageSize={pageSize}"
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Count().Should().Be(pageSize);
    }

    /// <summary>
    /// Verifies an OK response is returned containing the filtered products by
    /// category.
    /// </summary>
    [Fact]
    public async Task GetAllProducts_WithCategoryFilter_ReturnsOkContainingCorrectProducts()
    {
        var fashionCategory = _seedCategory.GetByType(CategorySeedType.FASHION);

        var response = await _client.GetAsync(
            $"{_endpoint}?category={fashionCategory.Id}"
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        foreach (var product in responseContent)
        {
            product.Categories.Should().Contain(fashionCategory.Name);
        }
    }
}
