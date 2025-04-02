using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using Application.Common.Persistence.Repositories;

using Infrastructure.Common.Persistence;

using SharedKernel.ValueObjects;

namespace Infrastructure.Carriers;

internal sealed class CarrierRepository
    : BaseRepository<Carrier, CarrierId>, ICarrierRepository
{
    public CarrierRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Carrier?> FindByEmail(
        Email email,
        CancellationToken cancellationToken = default
    )
    {
        return FirstOrDefaultAsync(
            carrier => carrier.Email == email,
            cancellationToken
        );
    }
}
