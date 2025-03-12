using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Coupons.Commands.UpdateCoupon;
using Application.Coupons.Errors;
using Application.UnitTests.Coupons.Commands.TestUtils;

using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.UnitTests.TestUtils;

using SharedKernel.Extensions;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Commands.UpdateCoupon;

/// <summary>
/// Unit tests for the <see cref="UpdateCouponCommandHandler"/> handler.
/// </summary>
public class UpdateCouponCommandHandlerTests
{
    private readonly UpdateCouponCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICouponRepository> _mockCouponRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="UpdateCouponCommandHandlerTests"/> class.
    /// </summary>
    public UpdateCouponCommandHandlerTests()
    {
        _mockCouponRepository = new Mock<ICouponRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new UpdateCouponCommandHandler(
            _mockCouponRepository.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<UpdateCouponCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies when the coupon exists it is updated.
    /// </summary>
    [Fact]
    public async Task HandleUpdateCoupon_WithExistingCoupon_UpdatesIt()
    {
        var couponId = CouponId.Create(1);
        var coupon = CouponUtils.CreateCoupon(id: couponId);
        var command = UpdateCouponCommandUtils.CreateCommand(couponId: couponId.ToString());

        _mockCouponRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CouponId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(coupon);

        await _handler.Handle(command, default);

        coupon.Code.Should().Be(command.Code.ToUpperSnakeCase());
        coupon.UsageLimit.Should().Be(command.UsageLimit);
        coupon.MinPrice.Should().Be(command.MinPrice);
        coupon.Discount.Should().Be(command.Discount);
        coupon.AutoApply.Should().Be(command.AutoApply);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies when the coupon does not exist an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleUpdateCoupon_WithNonexistingCoupon_ThrowsError()
    {
        var command = UpdateCouponCommandUtils.CreateCommand();

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
