using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

namespace Application.Common.Persistence;

/// <summary>
/// Defines the contract for sale persistence operations.
/// </summary>
public interface ISaleRepository : IBaseRepository<Sale, SaleId>
{
}
