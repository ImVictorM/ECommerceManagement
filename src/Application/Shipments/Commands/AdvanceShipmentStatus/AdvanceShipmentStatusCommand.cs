using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Shipments.Commands.AdvanceShipmentStatus;

/// <summary>
/// Represents a command to advance a shipment status.
/// </summary>
/// <param name="ShipmentId">The shipment identifier.</param>
[Authorize(roleName: nameof(Role.Carrier))]
[Authorize(policyType: typeof(ShipmentCarrierPolicy<AdvanceShipmentStatusCommand>))]
public record AdvanceShipmentStatusCommand(string ShipmentId)
    : IRequestWithAuthorization<Unit>, IShipmentSpecificResource;
