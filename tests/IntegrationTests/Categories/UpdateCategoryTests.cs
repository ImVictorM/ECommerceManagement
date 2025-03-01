using Contracts.Categories;

using IntegrationTests.Categories.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Categories;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the update category feature.
/// </summary>
public class UpdateCategoryTests : BaseIntegrationTest
{
    private readonly ICategorySeed _seedCategory;

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

        _seedCategory = SeedManager.GetSeed<ICategorySeed>();
    }

    /// <summary>
    /// Verifies updating a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var idExistingCategory = _seedCategory
            .GetEntityId(CategorySeedType.JEWELRY)
            .ToString();

        var request = UpdateCategoryRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.UpdateCategory),
            new { id = idExistingCategory }
        );

        var response = await RequestService.CreateClient().PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies updating a category without the admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var idExistingCategory = _seedCategory
            .GetEntityId(CategorySeedType.JEWELRY)
            .ToString();

        var request = UpdateCategoryRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.UpdateCategory),
            new { id = idExistingCategory }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies updating a category with the admin role updates the category correctly.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithAdminPermission_ReturnsNoContent()
    {
        var idCategoryToBeUpdated = _seedCategory
            .GetEntityId(CategorySeedType.JEWELRY)
            .ToString();

        var request = UpdateCategoryRequestUtils.CreateRequest(
            name: "new_category_name"
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.UpdateCategory),
            new { id = idCategoryToBeUpdated }
        );

        var endpointGetById = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.GetCategoryById),
            new { id = idCategoryToBeUpdated }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseUpdate = await client.PutAsJsonAsync(
            endpointUpdate,
            request
        );

        var responseGetUpdated = await client.GetAsync(endpointGetById);

        var responseGetUpdatedContent = await responseGetUpdated.Content
            .ReadRequiredFromJsonAsync<CategoryResponse>();

        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetUpdated.StatusCode.Should().Be(HttpStatusCode.OK);
        responseGetUpdatedContent.Name.Should().Be(request.Name);
    }
}
