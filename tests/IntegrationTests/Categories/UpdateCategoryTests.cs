using Domain.CategoryAggregate;

using Contracts.Categories;

using WebApi.Categories;

using IntegrationTests.Categories.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of updating a category.
/// </summary>
public class UpdateCategoryTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateCategoryTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {

        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
    }

    /// <summary>
    /// Tests updating a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest();

        var response = await RequestService.Client.PutAsJsonAsync($"{CategoryEndpoints.BaseEndpoint}/{existingCategory.Id}", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests updating a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest();

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PutAsJsonAsync($"{CategoryEndpoints.BaseEndpoint}/{existingCategory.Id}", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests updating a category with admin role updates the category correctly.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithAdminPermission_ReturnsCreate()
    {
        var categoryToBeUpdated = _seedCategory.GetByType(CategorySeedType.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest(name: "new_category_name");

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var updateResponse = await RequestService.Client
            .PutAsJsonAsync($"{CategoryEndpoints.BaseEndpoint}/{categoryToBeUpdated.Id}", request);
        var getUpdatedCategoryResponse = await RequestService.Client
            .GetAsync($"{CategoryEndpoints.BaseEndpoint}/{categoryToBeUpdated.Id}");

        var updatedCategory = await getUpdatedCategoryResponse.Content.ReadRequiredFromJsonAsync<CategoryResponse>();

        updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        getUpdatedCategoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        updatedCategory.Name.Should().Be(request.Name);
    }
}
