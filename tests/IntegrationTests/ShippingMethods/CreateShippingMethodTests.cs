using Contracts.ShippingMethods;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.ShippingMethods.TestUtils;

using WebApi.ShippingMethods;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the create shipping method feature.
/// </summary>
public class CreateShippingMethodTests : BaseIntegrationTest
{
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateShippingMethodTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.CreateShippingMethod
        ));
    }

    /// <summary>
    /// Verifies creating a shipping method without authentication
    /// returns unauthorized.
    /// </summary>
    [Fact]
    public async Task CreateShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = CreateShippingMethodRequestUtils.CreateRequest();

        var response = await RequestService.CreateClient().PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies creating a shipping method without admin role returns
    /// a forbidden response.
    /// </summary>
    [Fact]
    public async Task CreateShippingMethod_WithoutAdminPermission_ReturnsForbidden()
    {
        var request = CreateShippingMethodRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies creating a shipping method with admin role creates
    /// the shipping method correctly.
    /// </summary>
    [Fact]
    public async Task CreateShippingMethod_WithAdminPermission_ReturnsCreated()
    {
        var request = CreateShippingMethodRequestUtils.CreateRequest(
            name: "SuperFastDelivery",
            estimatedDeliveryDays: 4,
            price: 20m
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseCreate = await client.PostAsJsonAsync(
            _endpoint,
            request
        );
        var createdResourceLocation = responseCreate.Headers.Location;

        var getCreatedResponse = await client.GetAsync(createdResourceLocation);

        var createdShippingMethod = await getCreatedResponse.Content
            .ReadRequiredFromJsonAsync<ShippingMethodResponse>();

        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        createdShippingMethod.Id.Should().NotBeNullOrWhiteSpace();
        createdShippingMethod.Name.Should().Be(request.Name);
        createdShippingMethod.EstimatedDeliveryDays
            .Should()
            .Be(request.EstimatedDeliveryDays);
        createdShippingMethod.Price.Should().Be(request.Price);
    }
}
