using Domain.CategoryAggregate;

using Contracts.Categories;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Categories;

using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the get category by id feature.
/// </summary>
public class GetCategoryByIdTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

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
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
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

        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies an OK response is returned when a category with the specified
    /// identifier exists.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WithExistingCategoryId_ReturnsOk()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.TECHNOLOGY);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.GetCategoryById),
            new { id = existingCategory.Id.ToString() }
        );

        var response = await RequestService.Client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<CategoryResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Id.Should().Be(existingCategory.Id.ToString());
        responseContent.Name.Should().Be(existingCategory.Name);
    }
}
