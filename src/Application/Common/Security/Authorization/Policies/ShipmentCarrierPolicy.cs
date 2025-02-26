using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

using Domain.CarrierAggregate.ValueObjects;
using Domain.ShipmentAggregate.ValueObjects;

namespace Application.Common.Security.Authorization.Policies;

internal sealed class ShipmentCarrierPolicy<TRequest> : IPolicy<TRequest>
    where TRequest : IShipmentSpecificResource
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IShipmentRepository _shipmentRepository;

    public ShipmentCarrierPolicy(IIdentityProvider identityProvider, IShipmentRepository shipmentRepository)
    {
        _identityProvider = identityProvider;
        _shipmentRepository = shipmentRepository;
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthorizedAsync(TRequest request, IdentityUser currentUser)
    {
        var identity = _identityProvider.GetCurrentUserIdentity();

        var carrierId = CarrierId.Create(identity.Id);
        var shipmentId = ShipmentId.Create(request.ShipmentId);
        var shipment = await _shipmentRepository.FindByIdAsync(shipmentId);

        if (shipment == null)
        {
            // Let the use case handle scenarios where the shipment is null
            return true;
        }

        return shipment.CarrierId == carrierId;
    }
}
