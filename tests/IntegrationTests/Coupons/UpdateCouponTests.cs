using Contracts.Coupons;
using Contracts.Coupons.Restrictions;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Coupons;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.TestUtils.Contracts;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Assertions;
using IntegrationTests.Coupons.TestUtils;

using WebApi.Coupons;

using SharedKernel.Extensions;

using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Coupons;

/// <summary>
/// Integration tests for the update coupon feature.
/// </summary>
public class UpdateCouponTests : BaseIntegrationTest
{
    private readonly ICouponSeed _seedCoupon;
    private readonly ICategorySeed _seedCategory;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="UpdateCouponTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateCouponTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCoupon = SeedManager.GetSeed<ICouponSeed>();
        _seedCategory = SeedManager.GetSeed<ICategorySeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to update a
    /// coupon without authentication.
    /// </summary>
    [Fact]
    public async Task UpdateCoupon_WithoutAuthentication_ReturnsUnauthorized()
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.UpdateCoupon),
            new { id = couponId }
        );

        var request = UpdateCouponRequestUtils.CreateRequest();

        var response = await RequestService
            .CreateClient()
            .PutAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to update a coupon without the admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer type to be authenticated.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task UpdateCoupon_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.UpdateCoupon),
            new { id = couponId }
        );

        var request = UpdateCouponRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(customerType);
        var response = await client.PutAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is possible to update a coupon as an administrator.
    /// </summary>
    [Fact]
    public async Task UpdateCoupon_WithAdminAuthentication_ReturnsNoContent()
    {
        var couponId = _seedCoupon
            .GetEntityId(CouponSeedType.TECH_COUPON)
            .ToString();

        var endpointUpdateCoupon = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.UpdateCoupon),
            new { id = couponId }
        );

        var endpointGetCoupons = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.GetCoupons)
        );

        var request = UpdateCouponRequestUtils.CreateRequest(
            discount: DiscountContractUtils.CreateDiscount(
                percentage: 10,
                description: "Ten percent discount",
                startingDate: DateTimeOffset.UtcNow.AddMinutes(-60),
                endingDate: DateTimeOffset.UtcNow.AddDays(20)
            ),
            code: "PROMO10",
            usageLimit: 500,
            autoApply: true,
            minPrice: 0m,
            restrictions:
            [
                new CouponCategoryRestriction([
                    _seedCategory
                        .GetEntityId(CategorySeedType.TECHNOLOGY)
                        .ToString()
                ])
            ]
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseUpdate = await client.PutAsJsonAsync(
            endpointUpdateCoupon, request
        );

        var responseGet = await client.GetAsync(endpointGetCoupons);
        var responseGetContent = await responseGet.Content
            .ReadRequiredFromJsonAsync<IEnumerable<CouponResponse>>();

        var updatedCoupon = responseGetContent
            .FirstOrDefault(coupon => coupon.Id == couponId);

        var expectedRestrictions = request.Restrictions?.ToList();

        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        updatedCoupon.Should().NotBeNull();

        updatedCoupon!.Code.Should().Be(request.Code.ToUpperSnakeCase());
        updatedCoupon.Discount
            .Should()
            .BeEquivalentTo(request.Discount, options =>
                options.ComparingWithDateTimeOffset()
            );
        updatedCoupon.MinPrice.Should().Be(request.MinPrice);
        updatedCoupon.UsageLimit.Should().Be(request.UsageLimit);
        updatedCoupon.AutoApply.Should().Be(request.AutoApply);
        updatedCoupon.Restrictions.Should().BeEquivalentTo(
            expectedRestrictions,
            options => options
            .RespectingRuntimeTypes()
            .Using<IEnumerable<string>>(ctx => ctx.Subject
                .Should()
                .BeEquivalentTo(ctx.Expectation ?? [])
            )
            .WhenTypeIs<IEnumerable<string>>()
        );
    }
}
