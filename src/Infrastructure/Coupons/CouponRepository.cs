using Application.Common.Persistence.Repositories;
using Application.Coupons.DTOs;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Enumerations;

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

    public async Task<IEnumerable<Coupon>> GetCouponsAsync(
        CouponFilters filters,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable();

        if (filters.Active.HasValue)
        {
            query = query.Where(c => c.IsActive == filters.Active);
        }

        if (filters.ExpiringAfter.HasValue)
        {
            query = query.Where(c => c.Discount.EndingDate > filters.ExpiringAfter);
        }

        if (filters.ExpiringBefore.HasValue)
        {
            query = query.Where(c => c.Discount.EndingDate < filters.ExpiringBefore);
        }

        if (filters.ValidForDate.HasValue)
        {
            var date = filters.ValidForDate.Value;

            query = query.Where(c =>
                c.Discount.StartingDate <= date
                && c.Discount.EndingDate >= date
            );
        }

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
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

    public async Task<int> GetCouponUsageCountAsync(
        CouponId couponId,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Orders
            .Where(o =>
                o.CouponsApplied.Select(c => c.CouponId).Contains(couponId)
                && o.OrderStatus != OrderStatus.Canceled
            )
            .CountAsync(cancellationToken);
    }
}
