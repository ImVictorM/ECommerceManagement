using Domain.ShippingMethodAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
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
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteShippingMethodTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteShippingMethodTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
    }

    /// <summary>
    /// Verifies deleting a shipping method without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(
            ShippingMethodSeedType.FREE
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.DeleteShippingMethod),
            new { id = existingShippingMethod.Id.ToString() }
        );

        var response = await RequestService.Client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies deleting a shippingMethod without admin role returns
    /// a forbidden response.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(
            ShippingMethodSeedType.FREE
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.DeleteShippingMethod),
            new { id = existingShippingMethod.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies deleting a shipping method with admin role deletes the shipping method correctly.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithAdminPermission_ReturnsNoContent()
    {
        var shippingMethodToDelete = _seedShippingMethod.GetByType(
            ShippingMethodSeedType.FREE
        );

        var endpointDelete = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.DeleteShippingMethod),
            new { id = shippingMethodToDelete.Id.ToString() }
        );

        var endpointGetShippingMethodById = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.GetShippingMethodById),
            new { id = shippingMethodToDelete.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseDelete = await RequestService.Client.DeleteAsync(
            endpointDelete
        );
        var responseGetDeleted = await RequestService.Client.GetAsync(
            endpointGetShippingMethodById
        );

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetDeleted.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
