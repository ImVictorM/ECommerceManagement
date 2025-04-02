using Contracts.Sales;

using WebApi.Sales;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Sales;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Sales;

/// <summary>
/// Integration tests for the get sale by id feature.
/// </summary>
public class GetSaleByIdTests : BaseIntegrationTest
{
    private readonly ISaleSeed _seedSale;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetSaleByIdTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedSale = SeedManager.GetSeed<ISaleSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to get a
    /// sale without authentication.
    /// </summary>
    [Fact]
    public async Task GetSaleById_WithoutAuthentication_ReturnsUnauthorized()
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSaleById),
            new { id = saleId }
        );

        var response = await RequestService.CreateClient().GetAsync(
            endpoint
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to get a sale without the admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer without the admin role type.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetSaleById_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSaleById),
            new { id = saleId }
        );

        var client = await RequestService.LoginAsAsync(customerType);
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies that trying to get a sale that does not exist returns not found.
    /// </summary>
    [Fact]
    public async Task GetSaleById_WithNonExistentSale_ReturnsNotFound()
    {
        var nonExistentSaleId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSaleById),
            new { id = nonExistentSaleId }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies that a sale can be retrieved successfully.
    /// </summary>
    [Fact]
    public async Task GetSaleById_WithAdminAuthentication_ReturnsOk()
    {
        var sale = _seedSale.GetEntity(SaleSeedType.TECH_SALE);
        var saleId = sale.Id.ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSaleById),
            new { id = saleId }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var saleResponse = await response.Content
            .ReadRequiredFromJsonAsync<SaleResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        saleResponse.Id.Should().Be(saleId);
    }
}
