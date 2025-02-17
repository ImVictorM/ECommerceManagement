using Contracts.ShippingMethods;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.ShippingMethods.TestUtils;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the process of creating a shipping method.
/// </summary>
public class CreateShippingMethodTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateShippingMethodTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests creating a shipping method without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task CreateShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = CreateShippingMethodRequestUtils.CreateRequest();
        var endpoint = TestConstants.ShippingMethodEndpoints.CreateShippingMethod;

        var response = await RequestService.Client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests creating a shipping method without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task CreateShippingMethod_WithoutAdminPermission_ReturnsForbidden()
    {
        var request = CreateShippingMethodRequestUtils.CreateRequest();
        var endpoint = TestConstants.ShippingMethodEndpoints.CreateShippingMethod;

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests creating a shipping method with admin role creates the shipping method correctly.
    /// </summary>
    [Fact]
    public async Task CreateShippingMethod_WithAdminPermission_ReturnsCreated()
    {
        var request = CreateShippingMethodRequestUtils.CreateRequest(
            name: "SuperFastDelivery",
            estimatedDeliveryDays: 4,
            price: 20m
        );
        var endpoint = TestConstants.ShippingMethodEndpoints.CreateShippingMethod;

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var createResponse = await RequestService.Client.PostAsJsonAsync(endpoint, request);
        var resourceLocation = createResponse.Headers.Location;
        var getCreatedResponse = await RequestService.Client.GetAsync(resourceLocation);
        var createdShippingMethod = await getCreatedResponse.Content.ReadRequiredFromJsonAsync<ShippingMethodResponse>();

        createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        createdShippingMethod.Id.Should().NotBeNullOrWhiteSpace();
        createdShippingMethod.Name.Should().Be(request.Name);
        createdShippingMethod.EstimatedDeliveryDays.Should().Be(request.EstimatedDeliveryDays);
        createdShippingMethod.Price.Should().Be(request.Price);
    }
}
