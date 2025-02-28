using Domain.ShippingMethodAggregate;

using Contracts.ShippingMethods;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
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
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateShippingMethodTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
    }

    /// <summary>
    /// Verifies updating a shipping method without authentication returns
    /// unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(
            ShippingMethodSeedType.EXPRESS
        );
        var request = UpdateShippingMethodRequestUtils.CreateRequest();
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.UpdateShippingMethod),
            new { id = existingShippingMethod.Id.ToString() }
        );

        var response = await RequestService.Client.PutAsJsonAsync(
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
        var existingShippingMethod = _seedShippingMethod.GetByType(
            ShippingMethodSeedType.EXPRESS
        );
        var request = UpdateShippingMethodRequestUtils.CreateRequest();
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.UpdateShippingMethod),
            new { id = existingShippingMethod.Id.ToString() }
        );


        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PutAsJsonAsync(
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
        var shippingMethodToUpdate = _seedShippingMethod.GetByType(
            ShippingMethodSeedType.EXPRESS
        );

        var request = UpdateShippingMethodRequestUtils.CreateRequest(
            name: "ECommerceExpressDelivery",
            price: 10m,
            estimatedDeliveryDays: 2
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.UpdateShippingMethod),
            new { id = shippingMethodToUpdate.Id.ToString() }
        );

        var endpointGetShippingMethodById = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.GetShippingMethodById),
            new { id = shippingMethodToUpdate.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var updateResponse = await RequestService.Client.PutAsJsonAsync(
            endpointUpdate,
            request
        );
        var updatedShippingMethodResponse = await RequestService.Client.GetAsync(
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
