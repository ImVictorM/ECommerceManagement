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

        var response = await Client.PostAsJsonAsync(CategoryEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests creating a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var request = CreateCategoryRequestUtils.CreateRequest();

        await Client.LoginAs(SeedAvailableUsers.Customer);
        var response = await Client.PostAsJsonAsync(CategoryEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests creating a category with admin role creates the category correctly.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithAdminPermission_ReturnsCreate()
    {
        var request = CreateCategoryRequestUtils.CreateRequest(name: "new_category");

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var createResponse = await Client.PostAsJsonAsync(CategoryEndpoints.BaseEndpoint, request);

        var resourceLocation = createResponse.Headers.Location;
        var getCreatedResponse = await Client.GetAsync(resourceLocation);
        var createdCategory = await getCreatedResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        createdCategory!.Id.Should().NotBeNullOrWhiteSpace();
        createdCategory.Name.Should().Be(request.Name);
    }
}
