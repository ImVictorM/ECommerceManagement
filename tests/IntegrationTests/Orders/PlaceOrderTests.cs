using Domain.ProductAggregate;
using Domain.ShipmentAggregate.Enumerations;
using Domain.SaleAggregate.ValueObjects;
using Domain.OrderAggregate.Enumerations;

using Contracts.Orders;

using SharedKernel.Interfaces;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.Common.Seeds.Sales;
using IntegrationTests.Common.Seeds.Coupons;
using IntegrationTests.Orders.TestUtils;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Orders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the place order feature.
/// </summary>
public class PlaceOrderTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;
    private readonly ICouponSeed _seedCoupon;
    private readonly IShippingMethodSeed _seedShippingMethod;
    private readonly ISaleSeed _seedSale;
    private readonly IUserSeed _seedUser;
    private readonly IDiscountService _discountService;
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public PlaceOrderTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
        _seedCoupon = SeedManager.GetSeed<ICouponSeed>();
        _seedShippingMethod = SeedManager.GetSeed<IShippingMethodSeed>();
        _seedSale = SeedManager.GetSeed<ISaleSeed>();
        _seedUser = SeedManager.GetSeed<IUserSeed>();

        _discountService = factory.Services.GetRequiredService<IDiscountService>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.PlaceOrder)
        );
    }

    /// <summary>
    /// Verifies trying to place an order without authentication returns
    /// an unauthorized response.
    /// </summary>
    [Fact]
    public async Task PlaceOrder_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = PlaceOrderRequestUtils.CreateRequest();

        var response = await RequestService.CreateClient().PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies trying to place an order without the customer role returns a
    /// forbidden response.
    /// </summary>
    /// <param name="userWithoutCustomerRoleType">
    /// The user without the customer role.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.OTHER_ADMIN)]
    public async Task PlaceOrder_WithoutCustomerRole_ReturnsForbidden(
        UserSeedType userWithoutCustomerRoleType
    )
    {
        var request = PlaceOrderRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(userWithoutCustomerRoleType);

        client.SetIdempotencyKeyHeader(Guid.NewGuid().ToString());

        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies authenticated customers can place orders.
    /// </summary>
    /// <param name="userWithCustomerRoleType">
    /// The customer placing the order.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task PlaceOrder_WithAuthenticatedCustomer_ReturnsCreated(
        UserSeedType userWithCustomerRoleType
    )
    {
        var orderOwner = _seedUser.GetEntity(userWithCustomerRoleType);
        var pencil = _seedProduct.GetEntity(ProductSeedType.PENCIL);
        var computer = _seedProduct.GetEntity(ProductSeedType.COMPUTER_ON_SALE);
        var couponApplied = _seedCoupon.GetEntity(CouponSeedType.TECH_COUPON);
        var shippingMethod = _seedShippingMethod.GetEntity(
            ShippingMethodSeedType.EXPRESS
        );

        var request = PlaceOrderRequestUtils.CreateRequest(
            shippingMethodId: shippingMethod.Id.ToString(),
            products:
            [
                new OrderLineItemRequest(pencil.Id.ToString(), 1),
                new OrderLineItemRequest(computer.Id.ToString(), 2),
            ],
            couponAppliedIds:
            [
                couponApplied.Id.ToString()
            ]
        );

        var expectedCreatedPencilResponse = new OrderLineItemResponse(
            pencil.Id.ToString(),
            1,
            pencil.BasePrice,
            CalculateExpectedPriceAfterApplyingSales(pencil)
        );

        var expectedCreatedComputerResponse = new OrderLineItemResponse(
            computer.Id.ToString(),
            2,
            computer.BasePrice,
            CalculateExpectedPriceAfterApplyingSales(computer)
        );

        var expectedComputerTotal =
            expectedCreatedComputerResponse.PurchasedPrice
            * expectedCreatedComputerResponse.Quantity;

        var expectedPencilTotal =
            expectedCreatedPencilResponse.PurchasedPrice
            * expectedCreatedPencilResponse.Quantity;

        var expectedProductsTotal = _discountService.CalculateDiscountedPrice(
            expectedComputerTotal + expectedPencilTotal,
            [couponApplied.Discount]
        );

        var expectedTotalPrice = expectedProductsTotal + shippingMethod.Price;

        var clientCustomer = await RequestService.LoginAsAsync(userWithCustomerRoleType);

        clientCustomer.SetIdempotencyKeyHeader(Guid.NewGuid().ToString());

        var responseCreate = await clientCustomer.PostAsJsonAsync(
            _endpoint,
            request
        );
        var createdResourceLocation = responseCreate.Headers.Location;

        var clientAdmin = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseGetCreated = await clientAdmin.GetAsync(createdResourceLocation);

        var responseGetCreatedContent = await responseGetCreated.Content
            .ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        responseCreate.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
        responseGetCreatedContent.Status
            .Should()
            .Be(OrderStatus.Pending.Name);
        responseGetCreatedContent
            .Should()
            .NotBeNull();
        responseGetCreatedContent.Products
            .Should()
            .Contain(expectedCreatedPencilResponse);
        responseGetCreatedContent.Products
            .Should()
            .Contain(expectedCreatedComputerResponse);
        responseGetCreatedContent.OwnerId
            .Should()
            .Be(orderOwner.Id.ToString());
        responseGetCreatedContent.Total
            .Should()
            .Be(expectedTotalPrice);
        responseGetCreatedContent.Payment
            .Should()
            .NotBeNull();
        responseGetCreatedContent.Shipment
            .Should()
            .NotBeNull();
        responseGetCreatedContent.Shipment.Status
            .Should()
            .Be(ShipmentStatus.Pending.Name);
        responseGetCreatedContent.Shipment.DeliveryAddress
            .Should()
            .BeEquivalentTo(request.DeliveryAddress);
        responseGetCreatedContent.Shipment.ShippingMethod.Name
            .Should()
            .Be(shippingMethod.Name);
        responseGetCreatedContent.Shipment.ShippingMethod.Price
            .Should()
            .Be(shippingMethod.Price);
        responseGetCreatedContent.Shipment.ShippingMethod.EstimatedDeliveryDays
            .Should()
            .Be(shippingMethod.EstimatedDeliveryDays);
    }

    /// <summary>
    /// Verifies when some of the ordered products does not have available items
    /// in inventory a bad request is returned.
    /// </summary>
    [Fact]
    public async Task PlaceOrder_WithUnavailableProducts_ReturnsBadRequest()
    {
        var pencil = _seedProduct.GetEntity(ProductSeedType.PENCIL);
        var quantityToBuy = pencil.Inventory.QuantityAvailable + 1;

        var request = PlaceOrderRequestUtils.CreateRequest(
            products: [
                new OrderLineItemRequest(pencil.Id.ToString(), quantityToBuy),
            ]
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        client.SetIdempotencyKeyHeader(Guid.NewGuid().ToString());

        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private decimal CalculateExpectedPriceAfterApplyingSales(Product product)
    {
        var productCategoryIds = product.ProductCategories
            .Select(c => c.CategoryId)
            .ToHashSet();

        var saleProduct = SaleProduct.Create(product.Id, productCategoryIds);

        var discounts = _seedSale
            .ListAll(s => s.IsProductInSale(saleProduct))
            .Select(s => s.Discount);

        return _discountService.CalculateDiscountedPrice(
            product.BasePrice,
            discounts
        );
    }
}
