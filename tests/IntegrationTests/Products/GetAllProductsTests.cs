using Domain.CategoryAggregate;
using Domain.ProductAggregate;

using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Constants;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for getting all the products.
/// </summary>
public class GetAllProductsTests : BaseIntegrationTest
{
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllProductsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetAllProductsTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
    }

    /// <summary>
    /// Tests that is possible to get all the products that are active.
    /// </summary>
    [Fact]
    public async Task GetAllProducts_WhenGettingAllProducts_ReturnsOkContainingTheActiveProducts()
    {
        var activeProduct = _seedProduct.ListAll(product => product.IsActive);
        var endpoint = TestConstants.ProductEndpoints.GetAllProducts;

        var response = await RequestService.Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(activeProduct);
    }

    /// <summary>
    /// Tests that is possible to get the products with pagination.
    /// </summary>
    [Fact]
    public async Task GetAllProducts_WhenGettingProductsWithPagination_ReturnsOkContainingProductsSubset()
    {
        var page = 1;
        var pageSize = 2;
        var endpoint = TestConstants.ProductEndpoints.GetAllProducts;

        var response = await RequestService.Client.GetAsync($"{endpoint}?page={page}&pageSize={pageSize}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Count().Should().Be(pageSize);
    }

    /// <summary>
    /// Tests that is possible to get the products with a category filter.
    /// </summary>
    [Fact]
    public async Task GetAllProducts_WithCategoryFilter_ReturnsOkContainingCorrectProducts()
    {
        var fashionCategory = _seedCategory.GetByType(CategorySeedType.FASHION);
        var endpoint = TestConstants.ProductEndpoints.GetAllProducts;

        var response = await RequestService.Client.GetAsync($"{endpoint}?category={fashionCategory.Id}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<ProductResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        foreach (var product in responseContent)
        {
            product.Categories.Should().Contain(fashionCategory.Name);
        }
    }
}
