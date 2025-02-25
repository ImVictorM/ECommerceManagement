using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using Application.Common.Persistence.Repositories;

using Infrastructure.Common.Persistence;

using SharedKernel.ValueObjects;

namespace Infrastructure.Carriers;

/// <summary>
/// Defines the implementation for carrier persistence operations.
/// </summary>
public sealed class CarrierRepository : BaseRepository<Carrier, CarrierId>, ICarrierRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CarrierRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public CarrierRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public Task<Carrier?> FindByEmail(Email email, CancellationToken cancellationToken = default)
    {
        return FirstOrDefaultAsync(carrier => carrier.Email == email, cancellationToken);
    }
}
