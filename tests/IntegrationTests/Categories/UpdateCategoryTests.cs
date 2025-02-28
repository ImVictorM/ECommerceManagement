using Domain.CategoryAggregate;

using Contracts.Categories;

using IntegrationTests.Categories.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Categories;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the update category feature.
/// </summary>
public class UpdateCategoryTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateCategoryTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {

        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
    }

    /// <summary>
    /// Verifies updating a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.UpdateCategory),
            new { id = existingCategory.Id.ToString() }
        );

        var response = await RequestService.Client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies updating a category without the admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.UpdateCategory),
            new { id = existingCategory.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies updating a category with the admin role updates the category correctly.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithAdminPermission_ReturnsNoContent()
    {
        var categoryToBeUpdated = _seedCategory.GetByType(
            CategorySeedType.JEWELRY
        );

        var request = UpdateCategoryRequestUtils.CreateRequest(
            name: "new_category_name"
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.UpdateCategory),
            new { id = categoryToBeUpdated.Id.ToString() }
        );

        var endpointGetById = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.GetCategoryById),
            new { id = categoryToBeUpdated.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseUpdate = await RequestService.Client.PutAsJsonAsync(
            endpointUpdate,
            request
        );

        var responseGetUpdated = await RequestService.Client.GetAsync(
            endpointGetById
        );

        var responseGetUpdatedContent = await responseGetUpdated.Content
            .ReadRequiredFromJsonAsync<CategoryResponse>();

        responseUpdate.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        responseGetUpdated.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseGetUpdatedContent.Name.Should().Be(request.Name);
    }
}
