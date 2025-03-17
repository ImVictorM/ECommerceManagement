using Application.Common.Persistence.Repositories;

using Application.Coupons.Services;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Services;

/// <summary>
/// Unit tests for the <see cref="CouponUsageService"/> service.
/// </summary>
public class CouponUsageServiceTests
{
    private readonly CouponUsageService _service;
    private readonly Mock<ICouponRepository> _mockCouponRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CouponUsageServiceTests"/>
    /// class.
    /// </summary>
    public CouponUsageServiceTests()
    {
        _mockCouponRepository = new Mock<ICouponRepository>();

        _service = new CouponUsageService(_mockCouponRepository.Object);
    }

    /// <summary>
    /// Verifies that when the current usage count is below the coupon's usage limit,
    /// <see cref="CouponUsageService.IsWithinUsageLimitAsync"/> returns true.
    /// </summary>
    [Fact]
    public async Task IsWithinUsageLimitAsync_WhenUsageCountIsBelowLimit_ReturnsTrue()
    {
        var coupon = CouponUtils.CreateCoupon(usageLimit: 10);
        var currentUsage = 5;

        _mockCouponRepository
            .Setup(repo => repo.GetCouponUsageCountAsync(
                coupon.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(currentUsage);

        var result = await _service.IsWithinUsageLimitAsync(coupon);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that when the current usage count equals the coupon's usage limit,
    /// <see cref="CouponUsageService.IsWithinUsageLimitAsync"/> returns false.
    /// </summary>
    [Fact]
    public async Task IsWithinUsageLimitAsync_WhenUsageCountEqualsLimit_ReturnsFalse()
    {
        var coupon = CouponUtils.CreateCoupon(usageLimit: 10);
        var currentUsage = coupon.UsageLimit;

        _mockCouponRepository
            .Setup(repo => repo.GetCouponUsageCountAsync(
                coupon.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(currentUsage);

        var result = await _service.IsWithinUsageLimitAsync(coupon);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that when the current usage count exceeds the coupon's usage limit,
    /// <see cref="CouponUsageService.IsWithinUsageLimitAsync"/> returns false.
    /// </summary>
    [Fact]
    public async Task IsWithinUsageLimitAsync_WhenUsageCountExceedsLimit_ReturnsFalse()
    {
        var coupon = CouponUtils.CreateCoupon(usageLimit: 10);
        var currentUsage = 11;

        _mockCouponRepository
            .Setup(repo => repo.GetCouponUsageCountAsync(
                coupon.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(currentUsage);

        var result = await _service.IsWithinUsageLimitAsync(coupon);

        result.Should().BeFalse();
    }
}
