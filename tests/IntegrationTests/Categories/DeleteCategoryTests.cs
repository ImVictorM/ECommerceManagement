using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using WebApi.Categories;

using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of deleting a category.
/// </summary>
public class DeleteCategoryTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteCategoryTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests deleting a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingCategory = CategorySeed.GetSeedCategory(SeedAvailableCategories.JEWELRY);

        var response = await Client.DeleteAsync($"{CategoryEndpoints.BaseEndpoint}/{existingCategory.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests deleting a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingCategory = CategorySeed.GetSeedCategory(SeedAvailableCategories.JEWELRY);

        await Client.LoginAs(SeedAvailableUsers.CUSTOMER);
        var response = await Client.DeleteAsync($"{CategoryEndpoints.BaseEndpoint}/{existingCategory.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests deleting a category with admin role deletes the category correctly.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithAdminPermission_ReturnsCreate()
    {
        var categoryToBeDeleted = CategorySeed.GetSeedCategory(SeedAvailableCategories.JEWELRY);

        await Client.LoginAs(SeedAvailableUsers.ADMIN);

        var deleteResponse = await Client.DeleteAsync($"{CategoryEndpoints.BaseEndpoint}/{categoryToBeDeleted.Id}");
        var getDeletedCategoryResponse = await Client.GetAsync($"{CategoryEndpoints.BaseEndpoint}/{categoryToBeDeleted.Id}");

        deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        getDeletedCategoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
