using Domain.CategoryAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;

using WebApi.Categories;

using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the delete category feature.
/// </summary>
public class DeleteCategoryTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteCategoryTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
    }

    /// <summary>
    /// Verifies deleting a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.DeleteCategory),
            new { id = existingCategory.Id.ToString() }
        );

        var response = await RequestService.CreateClient().DeleteAsync(
           endpoint
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies deleting a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.DeleteCategory),
            new { id = existingCategory.Id.ToString() }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies deleting a category with admin role deletes the category correctly.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithAdminPermission_ReturnsNoContent()
    {
        var categoryToBeDeleted = _seedCategory.GetByType(CategorySeedType.JEWELRY);

        var endpointDelete = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.DeleteCategory),
            new { id = categoryToBeDeleted.Id.ToString() }
        );

        var endpointGetById = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.GetCategoryById),
            new { id = categoryToBeDeleted.Id.ToString() }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var deleteResponse = await client.DeleteAsync(
            endpointDelete
        );

        var getDeletedCategoryResponse = await client.GetAsync(
            endpointGetById
        );

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getDeletedCategoryResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
