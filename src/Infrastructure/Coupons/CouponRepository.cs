using Application.Common.Persistence.Repositories;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Coupons;

internal sealed class CouponRepository
    : BaseRepository<Coupon, CouponId>, ICouponRepository
{
    public CouponRepository(IECommerceDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<Coupon>> GetCouponsByIdsAsync(
        IEnumerable<CouponId> couponIds,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet
            .Where(c => couponIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }
}
