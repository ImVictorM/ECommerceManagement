using Application.Common.Persistence.Repositories;

using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.ShippingMethods;

internal sealed class ShippingMethodRepository : BaseRepository<ShippingMethod, ShippingMethodId>, IShippingMethodRepository
{
    public ShippingMethodRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
