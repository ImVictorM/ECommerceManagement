using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Coupons;

/// <summary>
/// Defines a contract to provide seed data for coupons in the database.
/// </summary>
public interface ICouponSeed : IDataSeed<CouponSeedType, Coupon, CouponId>
{
}
