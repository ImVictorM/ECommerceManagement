using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Sales;
using IntegrationTests.Common.Seeds.Users;

using WebApi.Sales;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Sales;

/// <summary>
/// Integration tests for the delete sale feature.
/// </summary>
public class DeleteSaleTests : BaseIntegrationTest
{
    private readonly ISaleSeed _seedSale;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteSaleTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedSale = SeedManager.GetSeed<ISaleSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to delete a
    /// sale without authentication.
    /// </summary>
    [Fact]
    public async Task DeleteSale_WithoutAuthentication_ReturnsUnauthorized()
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.DeleteSale),
            new
            {
                id = saleId
            }
        );

        var response = await RequestService.CreateClient().DeleteAsync(
            endpoint
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to delete a sale without the admin role.
    /// </summary>
    /// <param name="customerType">The customer without the admin role type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task DeleteSale_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.DeleteSale),
            new
            {
                id = saleId
            }
        );

        var client = await RequestService.LoginAsAsync(customerType);

        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies deleting a non-existent sale returns not found.
    /// </summary>
    [Fact]
    public async Task DeleteSale_WithNonExistentSale_ReturnsNotFound()
    {
        var nonExistentSaleId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.DeleteSale),
            new
            {
                id = nonExistentSaleId
            }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies it's possible to delete a sale with admin authentication.
    /// </summary>
    [Fact]
    public async Task DeleteSale_WithAdminAuthentication_ReturnsNoContent()
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpointDelete = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.DeleteSale),
            new
            {
                id = saleId
            }
        );

        var endpointGetById = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSaleById),
            new
            {
                id = saleId
            });

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseDelete = await client.DeleteAsync(endpointDelete);
        var responseGetDeleted = await client.GetAsync(endpointGetById);

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetDeleted.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
