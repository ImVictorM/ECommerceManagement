using Application.Common.Persistence.Repositories;

using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Sales;

internal sealed class SaleRepository : BaseRepository<Sale, SaleId>, ISaleRepository
{
    public SaleRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
