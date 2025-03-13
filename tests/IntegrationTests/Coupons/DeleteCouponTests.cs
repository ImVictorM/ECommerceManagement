using Contracts.Coupons;

using WebApi.Coupons;

using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Coupons;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Coupons;

/// <summary>
/// Integration tests for the delete coupon feature.
/// </summary>
public class DeleteCouponTests : BaseIntegrationTest
{
    private readonly ICouponSeed _seedCoupon;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCouponTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeleteCouponTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCoupon = SeedManager.GetSeed<ICouponSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to delete a
    /// coupon without authentication.
    /// </summary>
    [Fact]
    public async Task DeleteCoupon_WithoutAuthentication_ReturnsUnauthorized()
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.DeleteCoupon),
            new { id = couponId }
        );

        var response = await RequestService
            .CreateClient()
            .DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to delete a coupon without the admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer type to be authenticated.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task DeleteCoupon_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.DeleteCoupon),
            new { id = couponId }
        );

        var client = await RequestService.LoginAsAsync(customerType);

        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is possible to delete a coupon authenticated as an
    /// administrator.
    /// </summary>
    [Fact]
    public async Task DeleteCoupon_WithAdminAuthentication_ReturnsNoContent()
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpointDeleteCoupon = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.DeleteCoupon),
            new { id = couponId }
        );
        var endpointGetCoupons = LinkGenerator.GetPathByName(
           nameof(CouponEndpoints.GetCoupons)
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseDelete = await client.DeleteAsync(endpointDeleteCoupon);
        var responseGetCoupons = await client.GetAsync(endpointGetCoupons);

        var responseGetCouponsContent = await responseGetCoupons.Content
            .ReadRequiredFromJsonAsync<IEnumerable<CouponResponse>>();

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetCouponsContent.Select(r => r.Id).Should().NotContain(couponId);
    }
}
