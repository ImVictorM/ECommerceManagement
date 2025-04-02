using Contracts.Coupons.Restrictions;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Coupons.TestUtils;
using IntegrationTests.TestUtils.Contracts;

using WebApi.Coupons;

using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Coupons;

/// <summary>
/// Integration tests for the create coupon feature.
/// </summary>
public class CreateCouponTests : BaseIntegrationTest
{
    private readonly string? _endpoint;
    private readonly ICategorySeed _seedCategory;
    private readonly IProductSeed _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCouponTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateCouponTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _endpoint = LinkGenerator.GetPathByName(
            nameof(CouponEndpoints.CreateCoupon)
        );

        _seedCategory = SeedManager.GetSeed<ICategorySeed>();
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to create a
    /// coupon without authentication.
    /// </summary>
    [Fact]
    public async Task CreateCoupon_WithoutAuthentication_ReturnUnauthorized()
    {
        var request = CreateCouponRequestUtils.CreateRequest();

        var response = await RequestService.CreateClient().PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to create a coupon without the admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer without the admin role type.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task CreateCoupon_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var request = CreateCouponRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(customerType);

        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it's possible to create coupons authenticated as an administrator
    /// and posting a valid request without coupon restrictions.
    /// </summary>
    [Fact]
    public async Task CreateCoupon_WithAdminAuthenticationWithoutRestrictions_ReturnsCreated()
    {
        var request = CreateCouponRequestUtils.CreateRequest(
            code: "SAVE20",
            discount: DiscountContractUtils.CreateDiscount(
                percentage: 20,
                description: "Black week coupon",
                startingDate: DateTimeOffset.UtcNow.AddDays(1),
                endingDate: DateTimeOffset.UtcNow.AddDays(8)
            ),
            usageLimit: 1500,
            autoApply: true,
            minPrice: 150m
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Verifies it's possible to create coupons authenticated as an administrator
    /// and posting a valid request with coupon restrictions.
    /// </summary>
    [Fact]
    public async Task CreateCoupon_WithAdminAuthenticationWithRestrictions_ReturnsCreated()
    {
        var request = CreateCouponRequestUtils.CreateRequest(
            code: "SAVE20",
            discount: DiscountContractUtils.CreateDiscount(
                percentage: 20,
                description: "Black week coupon",
                startingDate: DateTimeOffset.UtcNow.AddDays(1),
                endingDate: DateTimeOffset.UtcNow.AddDays(8)
            ),
            usageLimit: 1500,
            autoApply: true,
            minPrice: 150m,
            restrictions:
            [
                new CouponCategoryRestriction(
                    [
                        _seedCategory
                            .GetEntityId(CategorySeedType.TECHNOLOGY)
                            .ToString()
                    ],
                    [
                        _seedProduct
                            .GetEntityId(ProductSeedType.COMPUTER_ON_SALE)
                            .ToString()
                    ]
                ),
                new CouponProductRestriction([
                    _seedProduct
                        .GetEntityId(ProductSeedType.PENCIL)
                        .ToString()
                ])
            ]
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
