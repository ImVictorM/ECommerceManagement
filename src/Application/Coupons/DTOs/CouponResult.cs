using Domain.CouponAggregate;

namespace Application.Coupons.DTOs;

/// <summary>
/// Represents a coupon response.
/// </summary>
/// <param name="Coupon">The coupon.</param>
public record CouponResult(Coupon Coupon);
