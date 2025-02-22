using Application.Common.Persistence;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Coupons;

/// <summary>
/// Defines the implementation for coupon persistence operations.
/// </summary>
public sealed class CouponRepository : BaseRepository<Coupon, CouponId>, ICouponRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CouponRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public CouponRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
