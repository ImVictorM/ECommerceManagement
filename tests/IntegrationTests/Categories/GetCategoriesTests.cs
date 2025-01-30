using Contracts.Categories;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Seeds;

using WebApi.Categories;

using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of retrieving all the available categories.
/// </summary>
public class GetCategoriesTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoriesTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCategoriesTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests retrieving the categories returns them all.
    /// </summary>
    [Fact]
    public async Task GetCategories_WhenCalled_ReturnsOkContainingAllAvailableCategories()
    {
        var expectedCategories = CategorySeed
            .ListCategories()
            .Select(c => new CategoryResponse(c.Id.ToString(), c.Name));

        var response = await Client.GetAsync($"{CategoryEndpoints.BaseEndpoint}");
        var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryResponse>>();

        responseContent.Should().BeEquivalentTo(expectedCategories);
    }
}
