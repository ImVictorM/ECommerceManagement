using Domain.CategoryAggregate;

using Contracts.Categories;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Constants;

using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of retrieving a category by its identifier.
/// </summary>
public class GetCategoryByIdTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoryByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCategoryByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
    }

    /// <summary>
    /// Tests getting an category that does not exist returns not found.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WhenCategoryDoesNotExist_ReturnsNotFound()
    {
        var missingCategoryId = "111111";

        var response = await RequestService.Client.GetAsync(TestConstants.CategoryEndpoints.GetCategoryById(missingCategoryId));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests getting an existing category returns ok containing the category.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WhenCategoryExists_ReturnsOkContainingCategory()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.TECHNOLOGY);

        var response = await RequestService.Client.GetAsync(TestConstants.CategoryEndpoints.GetCategoryById(existingCategory.Id.ToString()));
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<CategoryResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Id.Should().Be(existingCategory.Id.ToString());
        responseContent.Name.Should().Be(existingCategory.Name);
    }
}
