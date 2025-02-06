using Domain.CategoryAggregate;

using Contracts.Categories;

using WebApi.Categories;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.TestUtils.Extensions.Http;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of retrieving all the available categories.
/// </summary>
public class GetCategoriesTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoriesTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCategoriesTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
    }

    /// <summary>
    /// Tests retrieving the categories returns them all.
    /// </summary>
    [Fact]
    public async Task GetCategories_WhenCalled_ReturnsOkContainingAllAvailableCategories()
    {
        var expectedCategories = _seedCategory
            .ListAll()
            .Select(c => new CategoryResponse(c.Id.ToString(), c.Name));

        var response = await RequestService.Client.GetAsync($"{CategoryEndpoints.BaseEndpoint}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<CategoryResponse>>();

        responseContent.Should().BeEquivalentTo(expectedCategories);
    }
}
