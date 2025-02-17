using Domain.ShippingMethodAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Constants;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the process of deleting a shipping method.
/// </summary>
public class DeleteShippingMethodTests : BaseIntegrationTest
{
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteShippingMethodTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteShippingMethodTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
    }

    /// <summary>
    /// Verifies deleting a shipping method without authentication returns unauthorized.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(ShippingMethodSeedType.FREE);
        var endpoint = TestConstants.ShippingMethodEndpoints.DeleteShippingMethod(existingShippingMethod.Id.ToString());

        var response = await RequestService.Client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies deleting a shippingMethod without admin role returns a forbidden response.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithoutAdminPermission_ReturnsForbidden()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(ShippingMethodSeedType.FREE);
        var endpoint = TestConstants.ShippingMethodEndpoints.DeleteShippingMethod(existingShippingMethod.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies deleting a shipping method with admin role deletes the shipping method correctly.
    /// </summary>
    [Fact]
    public async Task DeleteShippingMethod_WithAdminPermission_ReturnsNoContentAndDeletesIt()
    {
        var shippingMethodToDelete = _seedShippingMethod.GetByType(ShippingMethodSeedType.FREE);
        var deleteEndpoint = TestConstants.ShippingMethodEndpoints.DeleteShippingMethod(shippingMethodToDelete.Id.ToString());
        var getByIdEndpoint = TestConstants.ShippingMethodEndpoints.GetShippingMethodById(shippingMethodToDelete.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var deleteResponse = await RequestService.Client.DeleteAsync(deleteEndpoint);
        var getDeletedResponse = await RequestService.Client.GetAsync(getByIdEndpoint);

        deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        getDeletedResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
