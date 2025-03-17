using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Coupons.Errors;

using Domain.CouponAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Coupons.Commands.DeleteCoupon;

internal sealed partial class DeleteCouponCommandHandler
    : IRequestHandler<DeleteCouponCommand, Unit>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCouponCommandHandler(
        ICouponRepository couponRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteCouponCommandHandler> logger
    )
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteCouponCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCouponDeletion(request.CouponId);

        var couponId = CouponId.Create(request.CouponId);

        var coupon = await _couponRepository.FindByIdAsync(
            couponId,
            cancellationToken
        );

        if (coupon == null)
        {
            LogCouponNotFound();
            throw new CouponNotFoundException();
        }

        _couponRepository.RemoveOrDeactivate(coupon);

        await _unitOfWork.SaveChangesAsync();

        LogCouponDeletedSuccessfully();

        return Unit.Value;
    }
}
