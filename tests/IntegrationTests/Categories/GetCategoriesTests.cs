using Contracts.Categories;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Categories;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the get categories feature.
/// </summary>
public class GetCategoriesTests : BaseIntegrationTest
{
    private readonly ICategorySeed _seedCategory;
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoriesTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCategoriesTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<ICategorySeed>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.GetCategories)
        );
    }

    /// <summary>
    /// Verifies an OK response is returned containing all categories.
    /// </summary>
    [Fact]
    public async Task GetCategories_WithValidRequest_ReturnsOk()
    {
        var expectedCategories = _seedCategory
            .ListAll()
            .Select(c => new CategoryResponse(c.Id.ToString(), c.Name));

        var response = await RequestService.CreateClient().GetAsync(
            _endpoint
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<CategoryResponse>>();

        responseContent.Should().BeEquivalentTo(expectedCategories);
    }
}
