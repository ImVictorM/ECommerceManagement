using System.Net.Http.Json;
using Contracts.Products;
using Domain.ProductAggregate.Enumerations;
using FluentAssertions;
using IntegrationTests.Common;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integrations tests for getting the product categories.
/// </summary>
public class GetProductCategoriesTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetProductCategoriesTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests the endpoint returns all the available product categories.
    /// </summary>
    [Fact]
    public async Task GetProductCategories_WhenRequested_ReturnsOkContainingAllAvailableProductCategories()
    {
        var response = await Client.GetAsync("/products/categories");
        var responseContent = await response.Content.ReadFromJsonAsync<ProductCategoriesResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent!.Categories.Should().BeEquivalentTo(Category.List().Select(c => c.Name));
    }
}
