using Application.Common.Persistence.Repositories;

using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Sales;

/// <summary>
/// Defines the implementation for sale persistence operations.
/// </summary>
public sealed class SaleRepository : BaseRepository<Sale, SaleId>, ISaleRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="SaleRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public SaleRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
