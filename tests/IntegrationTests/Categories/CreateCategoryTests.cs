using Contracts.Categories;

using IntegrationTests.Categories.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Categories;

using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Categories;

/// <summary>
/// Integration tests for the create category feature.
/// </summary>
public class CreateCategoryTests : BaseIntegrationTest
{
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCategoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateCategoryTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _endpoint = LinkGenerator.GetPathByName(
            nameof(CategoryEndpoints.CreateCategory)
        );
    }

    /// <summary>
    /// Verifies creating a category without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = CreateCategoryRequestUtils.CreateRequest();

        var response = await RequestService
            .CreateClient()
            .PostAsJsonAsync(
                _endpoint,
                request
            );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies creating a category without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithoutAdminPermission_ReturnsForbidden()
    {
        var request = CreateCategoryRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies creating a category with admin role creates the category correctly.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithAdminPermission_ReturnsCreated()
    {
        var request = CreateCategoryRequestUtils.CreateRequest(name: "new_category");

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseCreate = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var createdResourceLocation = responseCreate.Headers.Location;

        var responseGetCreated = await client.GetAsync(
            createdResourceLocation
        );

        var responseGetCreatedContent = await responseGetCreated.Content
            .ReadRequiredFromJsonAsync<CategoryResponse>();

        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseGetCreatedContent.Id.Should().NotBeNullOrWhiteSpace();
        responseGetCreatedContent.Name.Should().Be(request.Name);
    }
}
