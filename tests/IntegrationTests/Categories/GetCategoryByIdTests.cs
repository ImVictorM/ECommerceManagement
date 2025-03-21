using Contracts.Categories;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Categories;

using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the get category by id feature.
/// </summary>
public class GetCategoryByIdTests : BaseIntegrationTest
{
    private readonly ICategorySeed _seedCategory;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoryByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCategoryByIdTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<ICategorySeed>();
        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies a not found response is returned when the category does not exist.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WithNonexistingCategoryId_ReturnsNotFound()
    {
        var nonexistingCategoryId = "111111";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.GetCategoryById),
            new { id = nonexistingCategoryId }
        );

        var response = await _client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies an OK response is returned when a category with the specified
    /// identifier exists.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WithExistingCategoryId_ReturnsOk()
    {
        var existingCategory = _seedCategory.GetEntity(CategorySeedType.TECHNOLOGY);
        var existingCategoryId = existingCategory.Id.ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.GetCategoryById),
            new { id = existingCategoryId }
        );

        var response = await _client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<CategoryResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Id.Should().Be(existingCategoryId);
        responseContent.Name.Should().Be(existingCategory.Name);
    }
}
