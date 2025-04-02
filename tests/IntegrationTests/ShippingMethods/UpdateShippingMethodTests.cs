using Contracts.ShippingMethods;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.ShippingMethods.TestUtils;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.ShippingMethods;

using System.Net.Http.Json;
using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the update shipping method feature.
/// </summary>
public class UpdateShippingMethodTests : BaseIntegrationTest
{
    private readonly IShippingMethodSeed _seedShippingMethod;
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodTests"/>
    /// class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateShippingMethodTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<IShippingMethodSeed>();
    }

    /// <summary>
    /// Verifies updating a shipping method without authentication returns
    /// unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var idExistentShippingMethod = _seedShippingMethod
            .GetEntityId(ShippingMethodSeedType.EXPRESS)
            .ToString();

        var request = UpdateShippingMethodRequestUtils.CreateRequest();
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.UpdateShippingMethod),
            new { id = idExistentShippingMethod }
        );

        var response = await RequestService.CreateClient().PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies updating a shipping method without admin role returns
    /// a forbidden response.
    /// </summary>
    [Fact]
    public async Task UpdateShippingMethod_WithoutAdminPermission_ReturnsForbidden()
    {
        var idExistentShippingMethod = _seedShippingMethod
            .GetEntityId(ShippingMethodSeedType.EXPRESS)
            .ToString();

        var request = UpdateShippingMethodRequestUtils.CreateRequest();
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.UpdateShippingMethod),
            new { id = idExistentShippingMethod }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies updating a shipping method with admin role updates the shipping
    /// method correctly.
    /// </summary>
    [Fact]
    public async Task UpdateShippingMethod_WithAdminPermission_ReturnsNoContent()
    {
        var idShippingMethodToUpdate = _seedShippingMethod
            .GetEntityId(ShippingMethodSeedType.EXPRESS)
            .ToString();

        var request = UpdateShippingMethodRequestUtils.CreateRequest(
            name: "ECommerceExpressDelivery",
            price: 10m,
            estimatedDeliveryDays: 2
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.UpdateShippingMethod),
            new { id = idShippingMethodToUpdate }
        );

        var endpointGetShippingMethodById = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.GetShippingMethodById),
            new { id = idShippingMethodToUpdate }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var updateResponse = await client.PutAsJsonAsync(
            endpointUpdate,
            request
        );
        var updatedShippingMethodResponse = await client.GetAsync(
            endpointGetShippingMethodById
        );

        var updatedShippingMethod = await updatedShippingMethodResponse.Content
            .ReadRequiredFromJsonAsync<ShippingMethodResponse>();

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        updatedShippingMethodResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedShippingMethod.Name.Should().Be(request.Name);
        updatedShippingMethod.Price.Should().Be(request.Price);
        updatedShippingMethod.EstimatedDeliveryDays
            .Should()
            .Be(request.EstimatedDeliveryDays);
    }
}
