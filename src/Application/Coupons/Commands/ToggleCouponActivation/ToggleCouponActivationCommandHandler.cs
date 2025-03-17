using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Coupons.Errors;

using Domain.CouponAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Coupons.Commands.ToggleCouponActivation;

internal sealed partial class ToggleCouponActivationCommandHandler
    : IRequestHandler<ToggleCouponActivationCommand, Unit>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleCouponActivationCommandHandler(
        ICouponRepository couponRepository,
        IUnitOfWork unitOfWork,
        ILogger<ToggleCouponActivationCommandHandler> logger
    )
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        ToggleCouponActivationCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingToggleCouponActivation(request.CouponId);

        var couponId = CouponId.Create(request.CouponId);

        var coupon = await _couponRepository.FindByIdAsync(
            couponId,
            cancellationToken
        );

        if (coupon == null)
        {
            LogCouponNotFound();
            throw new CouponNotFoundException(couponId);
        }

        LogCurrentCouponState(coupon.IsActive);

        coupon.ToggleActivation();

        LogCouponStateToggledTo(coupon.IsActive);

        await _unitOfWork.SaveChangesAsync();

        LogCouponStateChangedSuccessfully();

        return Unit.Value;
    }
}
