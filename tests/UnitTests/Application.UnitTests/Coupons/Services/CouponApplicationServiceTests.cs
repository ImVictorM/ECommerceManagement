using Application.Common.Persistence.Repositories;
using Application.Coupons.Errors;
using Application.Coupons.Services;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Services;

/// <summary>
/// Unit tests for the <see cref="CouponApplicationService"/> service.
/// </summary>
public class CouponApplicationServiceTests
{
    private readonly CouponApplicationService _service;
    private readonly Mock<ICouponRepository> _mockCouponRepository;
    private readonly Mock<IDiscountService> _mockDiscountService;

    /// <summary>
    /// Initiates a new instance of the <see cref="CouponApplicationServiceTests"/>
    /// class.
    /// </summary>
    public CouponApplicationServiceTests()
    {
        _mockCouponRepository = new Mock<ICouponRepository>();
        _mockDiscountService = new Mock<IDiscountService>();

        _service = new CouponApplicationService(
            _mockCouponRepository.Object,
            _mockDiscountService.Object
        );
    }

    /// <summary>
    /// Verifies the correct application of coupons to an order.
    /// </summary>
    [Fact]
    public async Task ApplyCouponsAsync_WithValidCoupons_AppliesDiscountCorrectly()
    {
        var order = CouponUtils.CreateCouponOrder();
        var coupons = CouponUtils.CreateCoupons(count: 2);
        var couponDiscounts = coupons.Select(c => c.Discount);
        var couponIds = coupons.Select(c => c.Id);
        var expectedDiscountedTotal = order.Total - 20m;

        _mockCouponRepository
            .Setup(repo => repo.GetCouponsByIdsAsync(
                couponIds,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(coupons);

        _mockDiscountService
            .Setup(service => service.CalculateDiscountedPrice(
                order.Total,
                couponDiscounts
            ))
            .Returns(expectedDiscountedTotal);

        var result = await _service.ApplyCouponsAsync(order, couponIds);

        result.Should().Be(expectedDiscountedTotal);
    }

    /// <summary>
    /// Verifies that an exception is thrown when an invalid coupon is applied.
    /// </summary>
    [Fact]
    public async Task ApplyCouponsAsync_WithInvalidCoupon_ThrowsError()
    {
        var order = CouponUtils.CreateCouponOrder();
        var invalidCouponIds = new List<CouponId> { CouponId.Create(999) };

        _mockCouponRepository
            .Setup(repo => repo.GetCouponsByIdsAsync(
                invalidCouponIds,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync([]);

        await FluentActions
            .Invoking(() => _service.ApplyCouponsAsync(order, invalidCouponIds))
            .Should()
            .ThrowAsync<CouponNotFoundException>();
    }

    /// <summary>
    /// Verifies that an exception is thrown when a coupon cannot be applied to the order.
    /// </summary>
    [Fact]
    public async Task ApplyCouponsAsync_WithInapplicableCoupon_ThrowsError()
    {
        var order = CouponUtils.CreateCouponOrder();
        var couponInactive = CouponUtils.CreateCoupon(
            id: CouponId.Create(1),
            active: false
        );
        var couponId = new List<CouponId> { couponInactive.Id };
        var coupons = new List<Coupon> { couponInactive };

        _mockCouponRepository
            .Setup(repo => repo.GetCouponsByIdsAsync(
                couponId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(coupons);

        await FluentActions
            .Invoking(() => _service.ApplyCouponsAsync(order, couponId))
            .Should()
            .ThrowAsync<CouponApplicationFailedException>();
    }
}
