using Domain.ProductAggregate;
using Domain.UserAggregate.ValueObjects;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.ProductFeedback.TestUtils;

using WebApi.ProductFeedback;

using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.ProductFeedback;

/// <summary>
/// Integration tests for the leave product feedback feature.
/// </summary>
public class LeaveProductFeedbackTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;
    private readonly IOrderSeed _seedOrder;
    private readonly IUserSeed _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="LeaveProductFeedbackTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public LeaveProductFeedbackTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
        _seedOrder = SeedManager.GetSeed<IOrderSeed>();
        _seedUser = SeedManager.GetSeed<IUserSeed>();
    }

    /// <summary>
    /// Verifies it is not possible to leave feedback without authentication.
    /// </summary>
    [Fact]
    public async Task LeaveProductFeedback_WithoutAuthorization_ReturnsUnauthorized()
    {
        var idExistingProduct = _seedProduct
            .GetEntityId(ProductSeedType.PENCIL)
            .ToString();

        var request = LeaveProductFeedbackRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductFeedbackEndpoints.LeaveProductFeedback),
            new { productId = idExistingProduct }
        );

        var response = await RequestService.CreateClient().PostAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to leave feedback without the customer role.
    /// </summary>
    [Fact]
    public async Task LeaveProductFeedback_WithoutCustomerRole_ReturnsForbidden()
    {
        var idExistingProduct = _seedProduct
            .GetEntityId(ProductSeedType.PENCIL)
            .ToString();

        var request = LeaveProductFeedbackRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductFeedbackEndpoints.LeaveProductFeedback),
            new { productId = idExistingProduct }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is not possible to leave feedback without having purchased the product.
    /// </summary>
    [Fact]
    public async Task LeaveProductFeedback_WithoutHavingPurchasedTheProduct_ReturnsForbidden()
    {
        var userLeavingFeedbackType = UserSeedType.CUSTOMER;
        var userLeavingFeedback = _seedUser.GetEntity(userLeavingFeedbackType);

        var productNotAllowedToLeaveFeedback = GetFirstProductNotPurchased(
            userLeavingFeedback.Id
        );

        var request = LeaveProductFeedbackRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductFeedbackEndpoints.LeaveProductFeedback),
            new { productId = productNotAllowedToLeaveFeedback.Id }
        );

        var client = await RequestService.LoginAsAsync(userLeavingFeedbackType);
        var response = await client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is possible to leave feedback when the customer has purchased the product.
    /// </summary>
    [Fact]
    public async Task LeaveProductFeedback_WhenTheCustomerPurchasedTheProduct_ReturnsCreated()
    {
        var userLeavingFeedbackType = UserSeedType.CUSTOMER;
        var userLeavingFeedback = _seedUser.GetEntity(userLeavingFeedbackType);

        var productToLeaveFeedback = GetFirstProductPurchased(userLeavingFeedback.Id);
        var request = LeaveProductFeedbackRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductFeedbackEndpoints.LeaveProductFeedback),
            new { productId = productToLeaveFeedback.Id }
        );

        var client = await RequestService.LoginAsAsync(userLeavingFeedbackType);
        var response = await client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    private Product GetFirstProductNotPurchased(UserId customerId)
    {
        var customerOrders = _seedOrder.ListAll(o => o.OwnerId == customerId);

        var productsNotPurchased = _seedProduct.ListAll(p =>
            customerOrders.Any(o => !o.Products.Select(p => p.ProductId).Contains(p.Id))
        );

        return productsNotPurchased[0];
    }

    private Product GetFirstProductPurchased(UserId customerId)
    {
        var customerOrders = _seedOrder.ListAll(o => o.OwnerId == customerId);

        var productsPurchased = _seedProduct.ListAll(p =>
            customerOrders.Any(o => o.Products.Select(p => p.ProductId).Contains(p.Id))
        );

        return productsPurchased[0];
    }
}
