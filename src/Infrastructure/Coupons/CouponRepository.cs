using Application.Common.Persistence.Repositories;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Coupons;

internal sealed class CouponRepository
    : BaseRepository<Coupon, CouponId>, ICouponRepository
{
    public CouponRepository(IECommerceDbContext dbContext)
        : base(dbContext)
    {
    }
}
