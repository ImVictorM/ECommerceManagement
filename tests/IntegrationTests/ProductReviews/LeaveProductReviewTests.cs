using Domain.ProductAggregate;
using Domain.UserAggregate.ValueObjects;

using WebApi.ProductReviews;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.ProductReviews.TestUtils;

using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.ProductReviews;

/// <summary>
/// Integration tests for the leave product review feature.
/// </summary>
public class LeaveProductReviewTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;
    private readonly IOrderSeed _seedOrder;
    private readonly IUserSeed _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="LeaveProductReviewTests"/>
    /// class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public LeaveProductReviewTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
        _seedOrder = SeedManager.GetSeed<IOrderSeed>();
        _seedUser = SeedManager.GetSeed<IUserSeed>();
    }

    /// <summary>
    /// Verifies it is not possible to leave a review without authentication.
    /// </summary>
    [Fact]
    public async Task LeaveProductReview_WithoutAuthorization_ReturnsUnauthorized()
    {
        var idExistentProduct = _seedProduct
            .GetEntityId(ProductSeedType.PENCIL)
            .ToString();

        var request = LeaveProductReviewRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductReviewEndpoints.LeaveProductReview),
            new { productId = idExistentProduct }
        );

        var response = await RequestService.CreateClient().PostAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to leave a review without the customer role.
    /// </summary>
    [Fact]
    public async Task LeaveProductReview_WithoutCustomerRole_ReturnsForbidden()
    {
        var idExistentProduct = _seedProduct
            .GetEntityId(ProductSeedType.PENCIL)
            .ToString();

        var request = LeaveProductReviewRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductReviewEndpoints.LeaveProductReview),
            new { productId = idExistentProduct }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is not possible to leave a review without having purchased
    /// the product.
    /// </summary>
    [Fact]
    public async Task LeaveProductReview_WithoutHavingPurchasedTheProduct_ReturnsForbidden()
    {
        var userLeavingReviewType = UserSeedType.CUSTOMER;
        var userLeavingReview = _seedUser.GetEntity(userLeavingReviewType);

        var productNotAllowedToLeaveReview = GetFirstProductNotPurchased(
            userLeavingReview.Id
        );

        var request = LeaveProductReviewRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductReviewEndpoints.LeaveProductReview),
            new { productId = productNotAllowedToLeaveReview.Id }
        );

        var client = await RequestService.LoginAsAsync(userLeavingReviewType);
        var response = await client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is possible to leave a review when the customer has purchased
    /// the product.
    /// </summary>
    [Fact]
    public async Task LeaveProductReview_WhenTheCustomerPurchasedTheProduct_ReturnsCreated()
    {
        var userLeavingReviewType = UserSeedType.CUSTOMER;
        var userLeavingReview = _seedUser.GetEntity(userLeavingReviewType);

        var productToLeaveReview = GetFirstProductPurchased(userLeavingReview.Id);
        var request = LeaveProductReviewRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductReviewEndpoints.LeaveProductReview),
            new { productId = productToLeaveReview.Id }
        );

        var client = await RequestService.LoginAsAsync(userLeavingReviewType);
        var response = await client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    private Product GetFirstProductNotPurchased(UserId customerId)
    {
        var customerOrders = _seedOrder.ListAll(o => o.OwnerId == customerId);

        var productsNotPurchased = _seedProduct.ListAll(p =>
            customerOrders.Any(o =>
                !o.Products.Select(p => p.ProductId).Contains(p.Id)
            )
        );

        return productsNotPurchased[0];
    }

    private Product GetFirstProductPurchased(UserId customerId)
    {
        var customerOrders = _seedOrder.ListAll(o => o.OwnerId == customerId);

        var productsPurchased = _seedProduct.ListAll(p =>
            customerOrders.Any(o =>
                o.Products.Select(p => p.ProductId).Contains(p.Id)
            )
        );

        return productsPurchased[0];
    }
}
