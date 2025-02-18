using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.ShippingMethods.Commands.UpdateShippingMethod;

/// <summary>
/// Represents a command to update a shipping method.
/// </summary>
/// <param name="ShippingMethodId">The shipping method id.</param>
/// <param name="Name">The shipping method name.</param>
/// <param name="Price">The shipping method price.</param>
/// <param name="EstimatedDeliveryDays">The shipping method estimated delivery days.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record UpdateShippingMethodCommand(
    string ShippingMethodId,
    string Name,
    decimal Price,
    int EstimatedDeliveryDays
) : IRequestWithAuthorization<Unit>;
