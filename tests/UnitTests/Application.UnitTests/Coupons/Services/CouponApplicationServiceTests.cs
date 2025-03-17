using Application.Common.Persistence.Repositories;
using Application.Coupons.Errors;
using Application.Coupons.Services;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.CouponAggregate.Services;
using Domain.UnitTests.TestUtils;
using Domain.ProductAggregate.ValueObjects;
using Domain.CategoryAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;

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
    private readonly Mock<ICouponUsageService> _mockCouponUsageService;

    /// <summary>
    /// Initiates a new instance of the <see cref="CouponApplicationServiceTests"/>
    /// class.
    /// </summary>
    public CouponApplicationServiceTests()
    {
        _mockCouponRepository = new Mock<ICouponRepository>();
        _mockDiscountService = new Mock<IDiscountService>();
        _mockCouponUsageService = new Mock<ICouponUsageService>();

        _service = new CouponApplicationService(
            _mockCouponUsageService.Object,
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

        _mockCouponUsageService
            .Setup(service => service.IsWithinUsageLimitAsync(
                It.IsAny<Coupon>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

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
    /// Verifies that an exception is thrown when an inactive coupon is applied.
    /// </summary>
    [Fact]
    public async Task ApplyCouponsAsync_WithInactiveCoupon_ThrowsError()
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

        _mockCouponUsageService
                .Setup(service => service.IsWithinUsageLimitAsync(
                    It.IsAny<Coupon>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(true);

        await FluentActions
            .Invoking(() => _service.ApplyCouponsAsync(order, couponId))
            .Should()
            .ThrowAsync<CouponApplicationFailedException>();
    }

    /// <summary>
    /// Verifies that <see cref="CouponApplicationService.IsCouponApplicableAsync"/>
    /// returns true for a valid coupon.
    /// </summary>
    [Fact]
    public async Task IsCouponApplicableAsync_WithValidCoupon_ReturnsTrue()
    {
        var order = CouponUtils.CreateCouponOrder(total: 200m);

        var coupon = CouponUtils.CreateCoupon(
            active: true,
            usageLimit: 10,
            minPrice: 100m,
            discount: DiscountUtils.CreateDiscountValidToDate()
        );

        _mockCouponUsageService
            .Setup(service => service.IsWithinUsageLimitAsync(
                coupon,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        var result = await _service.IsCouponApplicableAsync(coupon, order);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that <see cref="CouponApplicationService.IsCouponApplicableAsync"/>
    /// returns false when the coupon usage limit is exceeded.
    /// </summary>
    [Fact]
    public async Task IsCouponApplicableAsync_WithUsageLimitExceeded_ReturnsFalse()
    {
        var order = CouponUtils.CreateCouponOrder(total: 200m);
        var coupon = CouponUtils.CreateCoupon(
            active: true,
            usageLimit: 10,
            minPrice: 100m,
            discount: DiscountUtils.CreateDiscountValidToDate()
        );

        _mockCouponUsageService
            .Setup(service => service.IsWithinUsageLimitAsync(
                coupon, It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(false);

        var result = await _service.IsCouponApplicableAsync(coupon, order);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that <see cref="CouponApplicationService.IsCouponApplicableAsync"/>
    /// returns false when the coupon discount is not valid to date.
    /// </summary>
    [Fact]
    public async Task IsCouponApplicableAsync_WithDiscountNotValidToDate_ReturnsFalse()
    {
        var order = CouponUtils.CreateCouponOrder(total: 200m);

        var coupon = CouponUtils.CreateCoupon(
            active: true,
            usageLimit: 10,
            minPrice: 100m,
            discount: DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddDays(2),
                endingDate: DateTimeOffset.UtcNow.AddDays(4)
            )
        );

        _mockCouponUsageService
            .Setup(service => service.IsWithinUsageLimitAsync(
                coupon,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        var result = await _service.IsCouponApplicableAsync(coupon, order);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that <see cref="CouponApplicationService.IsCouponApplicableAsync"/>
    /// returns false when the order total is less than the coupon minimum price.
    /// </summary>
    [Fact]
    public async Task IsCouponApplicableAsync_WithTotalLessThanMinimum_ReturnsFalse()
    {
        var order = CouponUtils.CreateCouponOrder(total: 50m);

        var coupon = CouponUtils.CreateCoupon(
            active: true,
            usageLimit: 10,
            minPrice: 100m,
            discount: DiscountUtils.CreateDiscountValidToDate()
        );

        _mockCouponUsageService
            .Setup(service => service.IsWithinUsageLimitAsync(
                coupon,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        var result = await _service.IsCouponApplicableAsync(coupon, order);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that <see cref="CouponApplicationService.IsCouponApplicableAsync"/>
    /// returns false when any of the coupon restrictions were not met.
    /// </summary>
    [Fact]
    public async Task IsCouponApplicableAsync_WithRestrictionsNotMet_ReturnsFalse()
    {
        var productPurchasedId = ProductId.Create(1);
        var otherProductId = ProductId.Create(2);

        var order = CouponUtils.CreateCouponOrder(
            products:
            [
                CouponOrderProduct.Create(
                    productPurchasedId,
                    new HashSet<CategoryId>()
                    {
                        CategoryId.Create(2)
                    }
                )
            ],
            total: 50m
        );

        var coupon = CouponUtils.CreateCoupon(
            active: true,
            usageLimit: 10,
            minPrice: 100m,
            discount: DiscountUtils.CreateDiscountValidToDate()
        );

        coupon.AssignRestriction(CouponProductRestriction.Create([
            CouponProduct.Create(otherProductId)
        ]));

        _mockCouponUsageService
            .Setup(service => service.IsWithinUsageLimitAsync(
                coupon,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        var result = await _service.IsCouponApplicableAsync(coupon, order);

        result.Should().BeFalse();
    }
}
