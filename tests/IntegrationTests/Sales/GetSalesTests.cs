using Contracts.Sales;

using WebApi.Sales;

using IntegrationTests.Common.Seeds.Sales;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Sales;

/// <summary>
/// Integration tests for the get sales feature.
/// </summary>
public class GetSalesTests : BaseIntegrationTest
{
    private readonly ISaleSeed _seedSale;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetSalesTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedSale = SeedManager.GetSeed<ISaleSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to get sales
    /// without authentication.
    /// </summary>
    [Fact]
    public async Task GetSales_WithoutAuthentication_ReturnsUnauthorized()
    {
        var endpoint = LinkGenerator.GetPathByName(nameof(SaleEndpoints.GetSales));

        var response = await RequestService.CreateClient().GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to get sales without the admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer without the admin role type.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetSales_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var endpoint = LinkGenerator.GetPathByName(nameof(SaleEndpoints.GetSales));

        var client = await RequestService.LoginAsAsync(customerType);
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies that sales can be retrieved successfully.
    /// </summary>
    [Fact]
    public async Task GetSales_WithAdminAuthentication_ReturnsOk()
    {
        var allSaleIds = _seedSale
            .ListAll()
            .Select(s => s.Id.ToString());

        var endpoint = LinkGenerator.GetPathByName(nameof(SaleEndpoints.GetSales));

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<List<SaleResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Select(s => s.Id).Should().BeEquivalentTo(allSaleIds);
    }

    /// <summary>
    /// Verifies that filtering sales by expiration date after a given date
    /// returns the expected results.
    /// </summary>
    [Fact]
    public async Task GetSales_WithExpiringAfterFilter_ReturnsFilteredSales()
    {
        var expiringAfter = DateTimeOffset.UtcNow.AddDays(10);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSales),
            new
            {
                expiringAfter
            }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<List<SaleResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent
            .Should()
            .OnlyContain(coupon => coupon.Discount.EndingDate > expiringAfter);
    }

    /// <summary>
    /// Verifies that filtering sales by expiration date before a given date
    /// returns the expected results.
    /// </summary>
    [Fact]
    public async Task GetSales_WithExpiringBeforeFilter_ReturnsFilteredSales()
    {
        var expiringBefore = DateTimeOffset.UtcNow.AddDays(20);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSales),
            new
            {
                expiringBefore
            }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<List<SaleResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().OnlyContain(s => s.Discount.EndingDate < expiringBefore);
    }

    /// <summary>
    /// Verifies that filtering sales that are valid for a given date returns
    /// the expected results.
    /// </summary>
    [Fact]
    public async Task GetSales_WithValidForDateFilter_ReturnsFilteredSales()
    {
        var validForDate = DateTimeOffset.UtcNow.AddDays(15);
        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSales),
            new
            {
                validForDate
            }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<List<SaleResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().OnlyContain(s =>
            s.Discount.StartingDate <= validForDate
            && s.Discount.EndingDate >= validForDate
        );
    }

    /// <summary>
    /// Verifies that filtering sales with both "expiringAfter" and "expiringBefore"
    /// parameters works correctly.
    /// </summary>
    [Fact]
    public async Task GetSales_WithExpiringAfterAndExpiringBeforeFilter_ReturnsFilteredSales()
    {
        var expiringAfter = DateTimeOffset.UtcNow.AddDays(5);
        var expiringBefore = DateTimeOffset.UtcNow.AddDays(25);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSales),
            new
            {
                expiringAfter,
                expiringBefore
            }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<List<SaleResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().OnlyContain(s =>
            s.Discount.EndingDate > expiringAfter
            && s.Discount.EndingDate < expiringBefore
        );
    }
}
