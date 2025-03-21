using Application.Sales.Queries.GetSaleById;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Sales.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetSaleByIdQuery"/> class.
/// </summary>
public static class GetSaleByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetSaleByIdQuery"/> class.
    /// </summary>
    /// <param name="saleId">The sale identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="GetSaleByIdQuery"/> class.
    /// </returns>
    public static GetSaleByIdQuery CreateQuery(
        string? saleId = null
    )
    {
        return new GetSaleByIdQuery(
            saleId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
