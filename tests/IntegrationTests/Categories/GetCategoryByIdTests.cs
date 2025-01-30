using Contracts.Categories;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Seeds;

using WebApi.Categories;

using Xunit.Abstractions;
using System.Net.Http.Json;
using FluentAssertions;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of retrieving a category by its identifier.
/// </summary>
public class GetCategoryByIdTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoryByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCategoryByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests getting an category that does not exist returns not found.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WhenCategoryDoesNotExist_ReturnsNotFound()
    {
        var missingCategoryId = "111111";

        var response = await Client.GetAsync($"{CategoryEndpoints.BaseEndpoint}/{missingCategoryId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests getting an existing category returns ok containing the category.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WhenCategoryExists_ReturnsOkContainingCategory()
    {
        var existingCategory = CategorySeed.GetSeedCategory(SeedAvailableCategories.TECHNOLOGY);

        var response = await Client.GetAsync($"{CategoryEndpoints.BaseEndpoint}/{existingCategory.Id}");
        var responseContent = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent!.Id.Should().Be(existingCategory.Id.ToString());
        responseContent.Name.Should().Be(existingCategory.Name);
    }
}
