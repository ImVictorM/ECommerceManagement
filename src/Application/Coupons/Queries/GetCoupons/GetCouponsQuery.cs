using Application.Common.Security.Authorization.Requests;
using Application.Coupons.DTOs;

using SharedKernel.ValueObjects;

namespace Application.Coupons.Queries.GetCoupons;

/// <summary>
/// Represents a query to retrieve coupons with filtering.
/// </summary>
/// <param name="Filters">The filters to apply in the query.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetCouponsQuery(
    CouponFilters Filters
) : IRequestWithAuthorization<IEnumerable<CouponResult>>;

