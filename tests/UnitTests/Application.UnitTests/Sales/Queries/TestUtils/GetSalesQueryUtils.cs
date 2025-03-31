using Application.Sales.DTOs.Filters;
using Application.Sales.Queries.GetSales;

namespace Application.UnitTests.Sales.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetSalesQuery"/> class.
/// </summary>
public static class GetSalesQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetSalesQuery"/> class.
    /// </summary>
    /// <param name="filters">The sale filters.</param>
    /// <returns>
    /// A new instance of the <see cref="GetSalesQuery"/> class.
    /// </returns>
    public static GetSalesQuery CreateQuery(
        SaleFilters? filters = null
    )
    {
        return new GetSalesQuery(
            filters ?? new SaleFilters()
        );
    }
}
