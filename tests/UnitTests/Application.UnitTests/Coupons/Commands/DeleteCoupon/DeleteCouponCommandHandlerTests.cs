using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Coupons.Commands.DeleteCoupon;
using Application.UnitTests.Coupons.Commands.TestUtils;
using Application.Coupons.Errors;

using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Commands.DeleteCoupon;

/// <summary>
/// Unit tests for the <see cref="DeleteCouponCommandHandler"/> handler.
/// </summary>
public class DeleteCouponCommandHandlerTests
{
    private readonly DeleteCouponCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICouponRepository> _mockCouponRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCouponCommandHandlerTests"/> class.
    /// </summary>
    public DeleteCouponCommandHandlerTests()
    {
        _mockCouponRepository = new Mock<ICouponRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new DeleteCouponCommandHandler(
            _mockCouponRepository.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<DeleteCouponCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies when the coupon exists it is deleted.
    /// </summary>
    [Fact]
    public async Task HandleDeleteCoupon_WithExistingCoupon_DeletesIt()
    {
        var couponId = CouponId.Create(1);
        var coupon = CouponUtils.CreateCoupon(id: couponId);

        var command = DeleteCouponCommandUtils.CreateCommand(
            couponId: couponId.ToString()
        );

        _mockCouponRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CouponId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(coupon);

        await _handler.Handle(command, default);

        _mockCouponRepository.Verify(
            r => r.RemoveOrDeactivate(coupon),
            Times.Once()
        );

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies when the coupon does not exist an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleDeleteCoupon_WithNonexistingCoupon_ThrowsError()
    {
        var command = DeleteCouponCommandUtils.CreateCommand();

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
