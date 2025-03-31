using Application.Common.Persistence.Repositories;
using Application.Orders.Errors;
using Application.Orders.Services;

using Domain.CouponAggregate.Services;
using Domain.CouponAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Services;

/// <summary>
/// Unit tests for the <see cref="OrderPricingService"/> service.
/// </summary>
public class OrderPricingServiceTests
{
    private readonly OrderPricingService _service;
    private readonly Mock<IShippingMethodRepository> _mockShippingMethodRepository;
    private readonly Mock<ICouponApplicationService> _mockCouponApplicationService;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderPricingServiceTests"/>
    /// class.
    /// </summary>
    public OrderPricingServiceTests()
    {
        _mockShippingMethodRepository = new Mock<IShippingMethodRepository>();
        _mockCouponApplicationService = new Mock<ICouponApplicationService>();

        _service = new OrderPricingService(
            _mockShippingMethodRepository.Object,
            _mockCouponApplicationService.Object
        );
    }

    /// <summary>
    /// Verifies the calculation of the total price when no coupon is applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotalAsync_WithoutCouponsApplied_ReturnsCalculatedTotalCorrectly()
    {
        var shippingMethodId = ShippingMethodId.Create(1);
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(
            id: shippingMethodId
        );
        var lineItems = OrderUtils.CreateOrderLineItems(count: 5);
        var lineItemsTotal = lineItems.Sum(i => i.CalculateTransactionPrice());
        var expectedTotal = lineItemsTotal + shippingMethod.Price;

        _mockShippingMethodRepository
            .Setup(repo => repo.FindByIdAsync(
                shippingMethodId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethod);

        var result = await _service.CalculateTotalAsync(
            lineItems,
            shippingMethodId
        );

        result.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Verifies the calculation of the total price when coupons are applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotalAsync_WithCouponsApplied_ReturnsCalculatedTotalCorrectly()
    {
        var shippingMethodId = ShippingMethodId.Create(1);
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(
            id: shippingMethodId
        );
        var lineItems = OrderUtils.CreateOrderLineItems(count: 5);
        var lineItemsTotal = lineItems.Sum(i => i.CalculateTransactionPrice());
        var couponsApplied = OrderUtils.CreateOrderCoupons(count: 2);
        var totalWithCouponsApplied = lineItemsTotal - 20m;
        var expectedTotal = totalWithCouponsApplied + shippingMethod.Price;

        _mockShippingMethodRepository
            .Setup(repo => repo.FindByIdAsync(
                shippingMethodId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethod);

        _mockCouponApplicationService
            .Setup(service => service.ApplyCouponsAsync(
                It.IsAny<CouponOrder>(),
                It.IsAny<IEnumerable<CouponId>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(totalWithCouponsApplied);

        var result = await _service.CalculateTotalAsync(
            lineItems,
            shippingMethodId,
            couponsApplied
        );

        result.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Verifies an exception is thrown when an invalid shipping method is provided.
    /// </summary>
    [Fact]
    public async Task CalculateTotalAsync_WithInvalidShippingMethod_ThrowsError()
    {
        var shippingMethodId = ShippingMethodId.Create(999);
        var lineItems = OrderUtils.CreateOrderLineItems(count: 3);

        _mockShippingMethodRepository
            .Setup(repo => repo.FindByIdAsync(
                shippingMethodId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((ShippingMethod?)null);

        await FluentActions
            .Invoking(() => _service.CalculateTotalAsync(
                lineItems,
                shippingMethodId
            ))
            .Should()
            .ThrowAsync<InvalidShippingMethodException>();
    }
}
