using Domain.ShippingMethodAggregate;

using Contracts.ShippingMethods;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.ShippingMethods.TestUtils;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using System.Net.Http.Json;
using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the process of updating a shipping method.
/// </summary>
public class UpdateShippingMethodTests : BaseIntegrationTest
{
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateShippingMethodTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
    }

    /// <summary>
    /// Verifies updating a shipping method without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(ShippingMethodSeedType.EXPRESS);
        var request = UpdateShippingMethodRequestUtils.CreateRequest();
        var endpoint = TestConstants.ShippingMethodEndpoints.UpdateShippingMethod(existingShippingMethod.Id.ToString());

        var response = await RequestService.Client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies updating a shipping method without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task UpdateShippingMethod_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(ShippingMethodSeedType.EXPRESS);
        var request = UpdateShippingMethodRequestUtils.CreateRequest();
        var endpoint = TestConstants.ShippingMethodEndpoints.UpdateShippingMethod(existingShippingMethod.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies updating a shipping method with admin role updates the shipping method correctly.
    /// </summary>
    [Fact]
    public async Task UpdateShippingMethod_WithAdminPermission_ReturnsNoContentAndUpdatesIt()
    {
        var shippingMethodToUpdate = _seedShippingMethod.GetByType(ShippingMethodSeedType.EXPRESS);

        var request = UpdateShippingMethodRequestUtils.CreateRequest(
            name: "ECommerceExpressDelivery",
            price: 10m,
            estimatedDeliveryDays: 2
        );

        var updateEndpoint = TestConstants.ShippingMethodEndpoints.UpdateShippingMethod(shippingMethodToUpdate.Id.ToString());
        var getByIdEndpoint = TestConstants.ShippingMethodEndpoints.GetShippingMethodById(shippingMethodToUpdate.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var updateResponse = await RequestService.Client.PutAsJsonAsync(updateEndpoint, request);
        var updatedShippingMethodResponse = await RequestService.Client.GetAsync(getByIdEndpoint);

        var updatedShippingMethod = await updatedShippingMethodResponse.Content.ReadRequiredFromJsonAsync<ShippingMethodResponse>();

        updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        updatedShippingMethodResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        updatedShippingMethod.Name.Should().Be(request.Name);
        updatedShippingMethod.Price.Should().Be(request.Price);
        updatedShippingMethod.EstimatedDeliveryDays.Should().Be(request.EstimatedDeliveryDays);
    }
}
