using Application.Common.Persistence;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

using Domain.CarrierAggregate.ValueObjects;
using Domain.ShipmentAggregate.ValueObjects;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Represents a policy that checks if the current user is the shipment carrier.
/// </summary>
public sealed class ShipmentCarrierPolicy<TRequest> : IPolicy<TRequest>
    where TRequest : IShipmentSpecificResource
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IShipmentRepository _shipmentRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentCarrierPolicy{TRequest}"/> class.
    /// </summary>
    /// <param name="identityProvider">The identity provider.</param>
    /// <param name="shipmentRepository">The shipment repository.</param>
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
