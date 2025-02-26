using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using SharedKernel.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for carrier persistence operations.
/// </summary>
public interface ICarrierRepository : IBaseRepository<Carrier, CarrierId>
{
    /// <summary>
    /// Retrieves a carrier matching the specified email.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The carrier with the specified email.</returns>
    Task<Carrier?> FindByEmail(Email email, CancellationToken cancellationToken = default);
}
