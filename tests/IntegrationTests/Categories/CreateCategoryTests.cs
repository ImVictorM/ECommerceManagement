using Contracts.Categories;

using IntegrationTests.Categories.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Constants;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the category creation process.
/// </summary>
public class CreateCategoryTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateCategoryTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests creating a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = CreateCategoryRequestUtils.CreateRequest();

        var response = await RequestService.Client.PostAsJsonAsync(TestConstants.CategoryEndpoints.CreateCategory, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests creating a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var request = CreateCategoryRequestUtils.CreateRequest();

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PostAsJsonAsync(TestConstants.CategoryEndpoints.CreateCategory, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests creating a category with admin role creates the category correctly.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithAdminPermission_ReturnsCreate()
    {
        var request = CreateCategoryRequestUtils.CreateRequest(name: "new_category");

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var createResponse = await RequestService.Client.PostAsJsonAsync(TestConstants.CategoryEndpoints.CreateCategory, request);
        var resourceLocation = createResponse.Headers.Location;
        var getCreatedResponse = await RequestService.Client.GetAsync(resourceLocation);
        var createdCategory = await getCreatedResponse.Content.ReadRequiredFromJsonAsync<CategoryResponse>();

        createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        createdCategory.Id.Should().NotBeNullOrWhiteSpace();
        createdCategory.Name.Should().Be(request.Name);
    }
}
