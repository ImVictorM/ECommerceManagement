using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.Common.Seeds.Users;

using WebApi.ShippingMethods;

using System.Net;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the delete shipping method feature.
/// </summary>
public class DeleteShippingMethodTests : BaseIntegrationTest
{
    private readonly IShippingMethodSeed _seedShippingMethod;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteShippingMethodTests"/>
    /// class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteShippingMethodTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<IShippingMethodSeed>();
    }

    /// <summary>
    /// Verifies deleting a shipping method without authentication returns
    /// unauthorized.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var idExistentShippingMethod = _seedShippingMethod
            .GetEntityId(ShippingMethodSeedType.FREE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.DeleteShippingMethod),
            new { id = idExistentShippingMethod }
        );

        var response = await RequestService
            .CreateClient()
            .DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies deleting a shippingMethod without admin role returns
    /// a forbidden response.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithoutAdminPermission_ReturnsForbidden()
    {
        var idExistentShippingMethod = _seedShippingMethod
            .GetEntityId(ShippingMethodSeedType.FREE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.DeleteShippingMethod),
            new { id = idExistentShippingMethod }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies deleting a shipping method with admin role deletes the shipping
    /// method correctly.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithAdminPermission_ReturnsNoContent()
    {
        var idShippingMethodToDelete = _seedShippingMethod
            .GetEntityId(ShippingMethodSeedType.FREE)
            .ToString();

        var endpointDelete = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.DeleteShippingMethod),
            new { id = idShippingMethodToDelete }
        );

        var endpointGetShippingMethodById = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.GetShippingMethodById),
            new { id = idShippingMethodToDelete }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseDelete = await client.DeleteAsync(
            endpointDelete
        );
        var responseGetDeleted = await client.GetAsync(
            endpointGetShippingMethodById
        );

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetDeleted.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
