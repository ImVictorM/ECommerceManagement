using Domain.CategoryAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.TestUtils.Constants;

using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the process of deleting a category.
/// </summary>
public class DeleteCategoryTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteCategoryTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
    }

    /// <summary>
    /// Tests deleting a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);

        var response = await RequestService.Client.DeleteAsync(
            TestConstants.CategoryEndpoints.DeleteCategory(existingCategory.Id.ToString())
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests deleting a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingCategory = _seedCategory.GetByType(CategorySeedType.JEWELRY);

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        var response = await RequestService.Client.DeleteAsync(
            TestConstants.CategoryEndpoints.DeleteCategory(existingCategory.Id.ToString())
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests deleting a category with admin role deletes the category correctly.
    /// </summary>
    [Fact]
    public async Task DeleteCategory_WithAdminPermission_ReturnsCreate()
    {
        var categoryToBeDeleted = _seedCategory.GetByType(CategorySeedType.JEWELRY);

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var deleteResponse = await RequestService.Client
            .DeleteAsync(TestConstants.CategoryEndpoints.DeleteCategory(categoryToBeDeleted.Id.ToString()));
        var getDeletedCategoryResponse = await RequestService.Client
            .GetAsync(TestConstants.CategoryEndpoints.GetCategoryById(categoryToBeDeleted.Id.ToString()));

        deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        getDeletedCategoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
