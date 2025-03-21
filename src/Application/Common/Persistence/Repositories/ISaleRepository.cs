using Application.Sales.DTOs;

using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for sale persistence operations.
/// </summary>
public interface ISaleRepository : IBaseRepository<Sale, SaleId>
{
    /// <summary>
    /// Retrieves a subset of sales based on the specified filters.
    /// </summary>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A subset of filtered <see cref="Sale"/> objects.</returns>
    Task<IReadOnlyList<Sale>> GetSalesAsync(
        SaleFilters filters,
        CancellationToken cancellationToken = default
    );
}
