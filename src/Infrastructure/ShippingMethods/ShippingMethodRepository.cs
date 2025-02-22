using Application.Common.Persistence;

using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.ShippingMethods;

/// <summary>
/// Defines the implementation for shipping method persistence operations.
/// </summary>
public sealed class ShippingMethodRepository : BaseRepository<ShippingMethod, ShippingMethodId>, IShippingMethodRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="ShippingMethodRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ShippingMethodRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
