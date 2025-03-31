using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Coupons.Commands.ToggleCouponActivation;
using Application.Coupons.Errors;
using Application.UnitTests.Coupons.Commands.TestUtils;

using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Commands.ToggleCouponActivation;

/// <summary>
/// Unit tests for the <see cref="ToggleCouponActivationCommandHandler"/>
/// handler.
/// </summary>
public class ToggleCouponActivationCommandHandlerTests
{
    private readonly ToggleCouponActivationCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICouponRepository> _mockCouponRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="ToggleCouponActivationCommandHandlerTests"/> class.
    /// </summary>
    public ToggleCouponActivationCommandHandlerTests()
    {
        _mockCouponRepository = new Mock<ICouponRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new ToggleCouponActivationCommandHandler(
            _mockCouponRepository.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<ToggleCouponActivationCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies the coupon <see cref="Coupon.IsActive"/> property is toggled to
    /// false when the coupon is active.
    /// </summary>
    [Fact]
    public async Task HandleToggleCouponActivationCommand_WithActiveCoupon_TogglesToFalse()
    {
        var couponId = CouponId.Create(1);
        var coupon = CouponUtils.CreateCoupon(id: couponId);

        var command = ToggleCouponActivationCommandUtils.CreateCommand(
            couponId: couponId.ToString()
        );

        _mockCouponRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CouponId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(coupon);

        await _handler.Handle(command, default);

        coupon.IsActive.Should().BeFalse();

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies the coupon <see cref="Coupon.IsActive"/> property is toggled to
    /// true when the coupon is inactive.
    /// </summary>
    [Fact]
    public async Task HandleToggleCouponActivationCommand_WithInactiveCoupon_TogglesToTrue()
    {
        var couponId = CouponId.Create(1);
        var coupon = CouponUtils.CreateCoupon(
            id: couponId,
            active: false
        );

        var command = ToggleCouponActivationCommandUtils.CreateCommand(
            couponId: couponId.ToString()
        );

        _mockCouponRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CouponId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(coupon);

        await _handler.Handle(command, default);

        coupon.IsActive.Should().BeTrue();

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies when the coupon does not exist an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleToggleCouponActivationCommand_WithNonexistingCoupon_ThrowsError()
    {
        var command = ToggleCouponActivationCommandUtils.CreateCommand();

        _mockCouponRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CouponId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Coupon?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<CouponNotFoundException>();
    }
}
