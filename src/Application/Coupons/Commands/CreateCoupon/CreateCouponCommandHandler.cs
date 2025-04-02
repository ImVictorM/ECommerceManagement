using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Coupons.Extensions;
using Application.Common.DTOs.Results;

using Domain.CouponAggregate;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Coupons.Commands.CreateCoupon;

internal sealed partial class CreateCouponCommandHandler
    : IRequestHandler<CreateCouponCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICouponRepository _couponRepository;

    public CreateCouponCommandHandler(
        IUnitOfWork unitOfWork,
        ICouponRepository couponRepository,
        ILogger<CreateCouponCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _couponRepository = couponRepository;
        _logger = logger;
    }

    public async Task<CreatedResult> Handle(
        CreateCouponCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCouponCreation(request.Code);

        var coupon = Coupon.Create(
            request.Discount,
            request.Code,
            request.UsageLimit,
            request.MinPrice,
            request.AutoApply
        );

        LogCouponCreated();

        var restrictions = request.Restrictions
            .ParseRestrictions()
            .ToList();

        LogRestrictionsParsed(restrictions.Count);

        foreach (var restriction in restrictions)
        {
            LogAssigningRestriction(restriction.GetType().Name);

            coupon.AssignRestriction(restriction);
        }

        await _couponRepository.AddAsync(coupon);

        await _unitOfWork.SaveChangesAsync();

        var couponGeneratedId = coupon.Id.ToString();

        LogCouponCreatedAndSavedSuccessfully(couponGeneratedId);

        return CreatedResult.FromId(couponGeneratedId);
    }
}
