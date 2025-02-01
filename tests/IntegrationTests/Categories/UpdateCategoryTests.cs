using Contracts.Categories;

using IntegrationTests.Categories.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using WebApi.Categories;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of updating a category.
/// </summary>
public class UpdateCategoryTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateCategoryTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests updating a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingCategory = CategorySeed.GetSeedCategory(SeedAvailableCategories.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest();

        var response = await Client.PutAsJsonAsync($"{CategoryEndpoints.BaseEndpoint}/{existingCategory.Id}", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests updating a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingCategory = CategorySeed.GetSeedCategory(SeedAvailableCategories.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest();

        await Client.LoginAs(SeedAvailableUsers.CUSTOMER);
        var response = await Client.PutAsJsonAsync($"{CategoryEndpoints.BaseEndpoint}/{existingCategory.Id}", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests updating a category with admin role updates the category correctly.
    /// </summary>
    [Fact]
    public async Task UpdateCategory_WithAdminPermission_ReturnsCreate()
    {
        var categoryToBeUpdated = CategorySeed.GetSeedCategory(SeedAvailableCategories.JEWELRY);
        var request = UpdateCategoryRequestUtils.CreateRequest(name: "new_category_name");

        await Client.LoginAs(SeedAvailableUsers.ADMIN);

        var updateResponse = await Client.PutAsJsonAsync($"{CategoryEndpoints.BaseEndpoint}/{categoryToBeUpdated.Id}", request);
        var getUpdatedCategoryResponse = await Client.GetAsync($"{CategoryEndpoints.BaseEndpoint}/{categoryToBeUpdated.Id}");
        var updatedCategory = await getUpdatedCategoryResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        getUpdatedCategoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        updatedCategory!.Name.Should().Be(request.Name);
    }
}
