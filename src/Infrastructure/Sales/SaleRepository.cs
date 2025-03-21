using Application.Common.Persistence.Repositories;
using Application.Sales.DTOs;

using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sales;

internal sealed class SaleRepository : BaseRepository<Sale, SaleId>, ISaleRepository
{
    public SaleRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<Sale>> GetSalesAsync(
        SaleFilters filters,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable();

        if (filters.ExpiringAfter.HasValue)
        {
            query = query.Where(c => c.Discount.EndingDate > filters.ExpiringAfter);
        }

        if (filters.ExpiringBefore.HasValue)
        {
            query = query.Where(c => c.Discount.EndingDate < filters.ExpiringBefore);
        }

        if (filters.ValidForDate.HasValue)
        {
            var date = filters.ValidForDate.Value;

            query = query.Where(c =>
                c.Discount.StartingDate <= date
                && c.Discount.EndingDate >= date
            );
        }

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
