using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Coupons.Errors;
using Application.Coupons.Extensions;

using Domain.CouponAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Coupons.Commands.UpdateCoupon;

internal sealed partial class UpdateCouponCommandHandler
    : IRequestHandler<UpdateCouponCommand, Unit>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCouponCommandHandler(
        ICouponRepository couponRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateCouponCommandHandler> logger
    )
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        UpdateCouponCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCouponUpdate(request.CouponId);

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

        coupon.Update(
            request.Discount,
            request.Code,
            request.UsageLimit,
            request.MinPrice,
            request.AutoApply
        );

        LogCouponUpdated();

        coupon.ClearRestrictions();

        LogCouponFormerRestrictionsCleared();

        var restrictionsUpdated = request.Restrictions
            .ParseRestrictions()
            .ToList();

        foreach (var restriction in restrictionsUpdated)
        {
            LogAssigningNewRestriction(restriction.GetType().Name);
            coupon.AssignRestriction(restriction);
        }

        LogRestrictionsAssigned();

        await _unitOfWork.SaveChangesAsync();

        LogCouponUpdatedSuccessfully();

        return Unit.Value;
    }
}
