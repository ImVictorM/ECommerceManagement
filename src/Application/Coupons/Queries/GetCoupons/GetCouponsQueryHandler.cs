using Application.Common.Persistence.Repositories;
using Application.Coupons.DTOs;
using Application.Coupons.Extensions;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Coupons.Queries.GetCoupons;

internal sealed partial class GetCouponsQueryHandler
    : IRequestHandler<GetCouponsQuery, IEnumerable<CouponResult>>
{
    private readonly ICouponRepository _couponRepository;

    public GetCouponsQueryHandler(
        ICouponRepository couponRepository,
        ILogger<GetCouponsQueryHandler> logger
    )
    {
        _couponRepository = couponRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<CouponResult>> Handle(
        GetCouponsQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCouponsRetrieval(
            request.Filters.Active,
            request.Filters.ExpiringAfter,
            request.Filters.ExpiringBefore,
            request.Filters.ValidForDate
        );

        var coupons = await _couponRepository.GetCouponsAsync(
            request.Filters,
            cancellationToken
        );

        LogCouponsRetrievedSuccessfully(coupons.Count());

        var result = coupons.Select(coupon => new CouponResult(
            coupon.Id.ToString(),
            coupon.Discount,
            coupon.Code,
            coupon.UsageLimit,
            coupon.AutoApply,
            coupon.MinPrice,
            coupon.Restrictions.ParseRestrictions()
        ));

        return result;
    }
}
