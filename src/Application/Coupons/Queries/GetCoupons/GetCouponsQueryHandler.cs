using Application.Common.Persistence.Repositories;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.Coupons.DTOs.Results;

namespace Application.Coupons.Queries.GetCoupons;

internal sealed partial class GetCouponsQueryHandler
    : IRequestHandler<GetCouponsQuery, IReadOnlyList<CouponResult>>
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

    public async Task<IReadOnlyList<CouponResult>> Handle(
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

        LogCouponsRetrievedSuccessfully(coupons.Count);

        var result = coupons
            .Select(coupon => new CouponResult(coupon))
            .ToList();

        return result;
    }
}
