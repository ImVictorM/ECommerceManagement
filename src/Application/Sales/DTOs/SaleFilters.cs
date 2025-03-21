namespace Application.Sales.DTOs;

/// <summary>
/// Represents filtering criteria for sale queries.
/// </summary>
///  <param name="ExpiringAfter">
/// Filters sales that expire after the specified UTC date.
/// </param>
/// <param name="ExpiringBefore">
/// Filters sales that expire before the specified UTC date.
/// </param>
/// <param name="ValidForDate">
/// Filters sales that were/will be valid during a specific UTC date.
/// </param>
public record SaleFilters(
    DateTimeOffset? ExpiringAfter = null,
    DateTimeOffset? ExpiringBefore = null,
    DateTimeOffset? ValidForDate = null
);
