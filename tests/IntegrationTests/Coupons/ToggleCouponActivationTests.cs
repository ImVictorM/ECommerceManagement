using Contracts.Coupons;

using IntegrationTests.Common.Seeds.Coupons;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Coupons;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Coupons;

/// <summary>
/// Integration tests for the toggle coupon activation feature.
/// </summary>
public class ToggleCouponActivationTests : BaseIntegrationTest
{
    private readonly ICouponSeed _seedCoupon;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="ToggleCouponActivationTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public ToggleCouponActivationTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCoupon = SeedManager.GetSeed<ICouponSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to toggle coupon
    /// activation without authentication.
    /// </summary>
    [Fact]
    public async Task ToggleCouponActivation_WithoutAuthentication_ReturnsUnauthorized()
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.ToggleCouponActivation),
            new { id = couponId }
        );

        var response = await RequestService
            .CreateClient()
            .PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to toggle coupon activation without the
    /// admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer type to be authenticated.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task ToggleCouponActivation_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.ToggleCouponActivation),
            new { id = couponId }
        );

        var client = await RequestService.LoginAsAsync(customerType);
        var response = await client.PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is possible to toggle coupon activation as an administrator.
    /// </summary>
    [Fact]
    public async Task ToggleCouponActivation_WithAdminAuthentication_ReturnsNoContent()
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpointToggleCouponActivation = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.ToggleCouponActivation),
            new { id = couponId }
        );
        var endpointGetInactiveCoupons = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.GetCoupons),
            new
            {
                active = false
            }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseToggleActivation = await client.PatchAsync(
            endpointToggleCouponActivation,
            null
        );

        var responseGetInactiveCoupons = await client.GetAsync(
            endpointGetInactiveCoupons
        );

        var responseGetInactiveCouponsContent = await responseGetInactiveCoupons
            .Content.ReadRequiredFromJsonAsync<IEnumerable<CouponResponse>>();

        responseToggleActivation.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetInactiveCouponsContent
            .FirstOrDefault(r => r.Id == couponId)
            .Should()
            .NotBeNull();
    }
}
