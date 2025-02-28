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

using WebApi.Orders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;
    private readonly IDataSeed<CouponSeedType, Coupon> _seedCoupon;
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;
    private readonly IDataSeed<SaleSeedType, Sale> _seedSale;
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
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
        _seedCoupon = SeedManager.GetSeed<CouponSeedType, Coupon>();
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
        _seedSale = SeedManager.GetSeed<SaleSeedType, Sale>();
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

        var response = await RequestService.Client.PostAsJsonAsync(
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

        RequestService.Client.DefaultRequestHeaders.Add(
            "X-Idempotency-Key",
            Guid.NewGuid().ToString()
        );

        await RequestService.LoginAsAsync(userWithoutCustomerRoleType);
        var response = await RequestService.Client.PostAsJsonAsync(
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
        var pencil = _seedProduct.GetByType(ProductSeedType.PENCIL);
        var computer = _seedProduct.GetByType(ProductSeedType.COMPUTER_ON_SALE);
        var couponApplied = _seedCoupon.GetByType(CouponSeedType.TECH_COUPON);
        var shippingMethod = _seedShippingMethod.GetByType(
            ShippingMethodSeedType.EXPRESS
        );

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

        RequestService.Client.DefaultRequestHeaders.Add(
            "X-Idempotency-Key",
            Guid.NewGuid().ToString()
        );

        var orderOwner = await RequestService.LoginAsAsync(userWithCustomerRoleType);
        var responseCreate = await RequestService.Client.PostAsJsonAsync(
            _endpoint,
            request
        );
        var createdResourceLocation = responseCreate.Headers.Location;

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseGetCreated = await RequestService.Client.GetAsync(
            createdResourceLocation
        );

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
        var pencil = _seedProduct.GetByType(ProductSeedType.PENCIL);
        var quantityToBuy = pencil.Inventory.QuantityAvailable + 1;

        var request = PlaceOrderRequestUtils.CreateRequest(
            products: [
                new OrderProductRequest(pencil.Id.ToString(), quantityToBuy),
            ]
        );

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        RequestService.Client.DefaultRequestHeaders.Add(
            "X-Idempotency-Key",
            Guid.NewGuid().ToString()
        );

        var response = await RequestService.Client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Title.Should().Be("Product Not Available");
        responseContent.Detail.Should().Be(
            $"The product with id {pencil.Id} is not available at the moment"
        );
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
