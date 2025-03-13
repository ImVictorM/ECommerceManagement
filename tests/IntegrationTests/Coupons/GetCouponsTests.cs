using Contracts.Coupons;

using WebApi.Coupons;

using IntegrationTests.Common.Seeds.Coupons;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.AspNetCore.Routing;
using FluentAssertions;
using Xunit.Abstractions;
using System.Net;

namespace IntegrationTests.Coupons;

/// <summary>
/// Integration tests for the get coupons feature.
/// </summary>
public class GetCouponsTests : BaseIntegrationTest
{
    private readonly ICouponSeed _seedCoupon;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCouponsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCouponsTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCoupon = SeedManager.GetSeed<ICouponSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to get coupons
    /// without authentication.
    /// </summary>
    [Fact]
    public async Task GetCoupons_WithoutAuthentication_ReturnsUnauthorized()
    {
        var endpoint = LinkGenerator
            .GetPathByName(nameof(CouponEndpoints.GetCoupons));

        var response = await RequestService
            .CreateClient()
            .GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies that only admins can retrieve the list of available coupons.
    /// </summary>
    [Fact]
    public async Task GetCoupons_WithAdminAuthentication_ReturnsOk()
    {
        var endpoint = LinkGenerator
            .GetPathByName(nameof(CouponEndpoints.GetCoupons));

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<CouponResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeEmpty();
        responseContent.Should().HaveCount(_seedCoupon.ListAll().Count);
    }

    /// <summary>
    /// Verifies that a customer cannot retrieve the list of available coupons.
    /// </summary>
    [Fact]
    public async Task GetCoupons_WithCustomerAuthentication_ReturnsForbidden()
    {
        var endpoint = LinkGenerator
            .GetPathByName(nameof(CouponEndpoints.GetCoupons));

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies the filtering by expiration date using "expiringAfter".
    /// </summary>
    [Fact]
    public async Task GetCoupons_WithExpiringAfterFilter_ReturnsOnlyMatchingCoupons()
    {
        var expiringAfter = DateTimeOffset.UtcNow.AddDays(2);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.GetCoupons),
            new { expiringAfter }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<CouponResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent
            .Should()
            .OnlyContain(coupon => coupon.Discount.EndingDate > expiringAfter);
    }

    /// <summary>
    /// Verifies the filtering by expiration date using "expiringBefore".
    /// </summary>
    [Fact]
    public async Task GetCoupons_WithExpiringBeforeFilter_ReturnsOnlyMatchingCoupons()
    {
        var expiringBefore = DateTimeOffset.UtcNow.AddDays(7);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.GetCoupons),
            new { expiringBefore }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<CouponResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().OnlyContain(coupon =>
            coupon.Discount.EndingDate < expiringBefore);
    }

    /// <summary>
    /// Verifies that only coupons valid for a specific date are returned.
    /// </summary>
    [Fact]
    public async Task GetCoupons_WithValidForDateFilter_ReturnsOnlyMatchingCoupons()
    {
        var validForDate = DateTimeOffset.UtcNow.AddDays(1);
        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.GetCoupons),
            new { validForDate }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<CouponResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent
            .Should()
            .OnlyContain(coupon =>
                coupon.Discount.StartingDate <= validForDate
                && coupon.Discount.EndingDate >= validForDate
            );
    }
}
