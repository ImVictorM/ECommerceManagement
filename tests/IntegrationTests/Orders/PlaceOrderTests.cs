using Domain.ProductAggregate;
using Domain.CouponAggregate;
using Domain.ShippingMethodAggregate;
using Domain.ShipmentAggregate.Enumerations;
using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.OrderAggregate.Enumerations;

using Contracts.Orders;

using SharedKernel.Interfaces;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.Common.Seeds.Sales;
using IntegrationTests.Common.Seeds.Coupons;
using IntegrationTests.Orders.TestUtils;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Constants;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of placing an order.
/// </summary>
public class PlaceOrderTests : BaseIntegrationTest
{
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;
    private readonly IDataSeed<CouponSeedType, Coupon> _seedCoupon;
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;
    private readonly IDataSeed<SaleSeedType, Sale> _seedSale;
    private readonly IDiscountService _discountService;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public PlaceOrderTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
        _seedCoupon = SeedManager.GetSeed<CouponSeedType, Coupon>();
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
        _seedSale = SeedManager.GetSeed<SaleSeedType, Sale>();
        _discountService = factory.Services.GetRequiredService<IDiscountService>();
    }

    /// <summary>
    /// Tests when the user trying to place an order is not authenticated it is returned a unauthorized response.
    /// </summary>
    [Fact]
    public async Task PlaceOrder_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var request = PlaceOrderRequestUtils.CreateRequest();
        var endpoint = TestConstants.OrderEndpoints.PlaceOrder;

        var response = await RequestService.Client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that only authenticated customers can place orders.
    /// </summary>
    /// <param name="userWithoutCustomerRoleType">The user without the customer role.</param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.OTHER_ADMIN)]
    public async Task PlaceOrder_WhenUserDoesNotHaveCustomerRole_ReturnsForbidden(UserSeedType userWithoutCustomerRoleType)
    {
        var request = PlaceOrderRequestUtils.CreateRequest();
        var endpoint = TestConstants.OrderEndpoints.PlaceOrder;

        RequestService.Client.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());
        await RequestService.LoginAsAsync(userWithoutCustomerRoleType);
        var response = await RequestService.Client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests customers can place orders and the order is created correctly.
    /// </summary>
    /// <param name="userWithCustomerRoleType">The customer.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task PlaceOrder_WhenParametersAreCorrectAndUserIsAuthorized_CreatesOrderAndReturnsCreated(UserSeedType userWithCustomerRoleType)
    {
        var pencil = _seedProduct.GetByType(ProductSeedType.PENCIL);
        var computer = _seedProduct.GetByType(ProductSeedType.COMPUTER_ON_SALE);
        var couponApplied = _seedCoupon.GetByType(CouponSeedType.TECH_COUPON);
        var shippingMethod = _seedShippingMethod.GetByType(ShippingMethodSeedType.EXPRESS);

        var request = PlaceOrderRequestUtils.CreateRequest(
            shippingMethodId: shippingMethod.Id.ToString(),
            products:
            [
                new OrderProductRequest(pencil.Id.ToString(), 1),
                new OrderProductRequest(computer.Id.ToString(), 2),
            ],
            couponAppliedIds:
            [
                couponApplied.Id.ToString()
            ]
        );

        var expectedCreatedPencilResponse = new OrderProductResponse(
            pencil.Id.ToString(),
            1,
            pencil.BasePrice,
            CalculateExpectedPriceAfterApplyingSales(pencil)
        );

        var expectedCreatedComputerResponse = new OrderProductResponse(
            computer.Id.ToString(),
            2,
            computer.BasePrice,
            CalculateExpectedPriceAfterApplyingSales(computer)
        );

        var expectedComputerTotal = expectedCreatedComputerResponse.PurchasedPrice * expectedCreatedComputerResponse.Quantity;
        var expectedPencilTotal = expectedCreatedPencilResponse.PurchasedPrice * expectedCreatedPencilResponse.Quantity;

        var expectedTotalPrice = _discountService.CalculateDiscountedPrice(
            expectedComputerTotal + expectedPencilTotal,
            [couponApplied.Discount]
        ) + shippingMethod.Price;

        var endpoint = TestConstants.OrderEndpoints.PlaceOrder;

        RequestService.Client.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());
        var orderOwner = await RequestService.LoginAsAsync(userWithCustomerRoleType);
        var createResponse = await RequestService.Client.PostAsJsonAsync(endpoint, request);
        var resourceLocation = createResponse.Headers.Location;

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var getResourceResponse = await RequestService.Client.GetAsync(resourceLocation);
        var createdResourceContent = await getResourceResponse.Content.ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        createdResourceContent.Status.Should().Be(OrderStatus.Pending.Name);
        createdResourceContent.Should().NotBeNull();
        createdResourceContent.Products.Should().Contain(expectedCreatedPencilResponse);
        createdResourceContent.Products.Should().Contain(expectedCreatedComputerResponse);
        createdResourceContent.OwnerId.Should().Be(orderOwner.Id.ToString());
        createdResourceContent.Total.Should().Be(expectedTotalPrice);
        createdResourceContent.Payment.Should().NotBeNull();
        createdResourceContent.Shipment.Should().NotBeNull();
        createdResourceContent.Shipment.Status.Should().Be(ShipmentStatus.Pending.Name);
        createdResourceContent.Shipment.DeliveryAddress.Should().BeEquivalentTo(request.DeliveryAddress);
        createdResourceContent.Shipment.ShippingMethod.Name.Should().Be(shippingMethod.Name);
        createdResourceContent.Shipment.ShippingMethod.Price.Should().Be(shippingMethod.Price);
        createdResourceContent.Shipment.ShippingMethod.EstimatedDeliveryDays.Should().Be(shippingMethod.EstimatedDeliveryDays);
    }

    /// <summary>
    /// Tests when some of the products does not have available items in inventory the return is a bad request.
    /// </summary>
    [Fact]
    public async Task PlaceOrder_WhenSomeOfTheProductsDoesNotHaveAvailableItemsInInventory_ReturnsBadRequest()
    {
        var pencil = _seedProduct.GetByType(ProductSeedType.PENCIL);
        var quantityToBuy = pencil.Inventory.QuantityAvailable + 1;

        var request = PlaceOrderRequestUtils.CreateRequest(
            products: [
                new OrderProductRequest(pencil.Id.ToString(), quantityToBuy),
            ]
        );

        var endpoint = TestConstants.OrderEndpoints.PlaceOrder;

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        RequestService.Client.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());
        var response = await RequestService.Client.PostAsJsonAsync(endpoint, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Detail.Should().Be($"The product with id {pencil.Id} is not available at the moment");
        responseContent.Title.Should().Be("Product Not Available");
    }

    private decimal CalculateExpectedPriceAfterApplyingSales(Product product)
    {
        var productCategoryIds = product.ProductCategories.Select(c => c.CategoryId).ToHashSet();

        var saleProduct = SaleProduct.Create(product.Id, productCategoryIds);

        var discounts = _seedSale
            .ListAll(s => s.IsProductInSale(saleProduct))
            .Select(s => s.Discount);

        return _discountService.CalculateDiscountedPrice(product.BasePrice, discounts.ToArray());
    }
}
