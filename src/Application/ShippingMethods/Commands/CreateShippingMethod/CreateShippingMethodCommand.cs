using Application.Common.Security.Authorization.Requests;
using Application.Common.DTOs.Results;

using SharedKernel.ValueObjects;

namespace Application.ShippingMethods.Commands.CreateShippingMethod;

/// <summary>
/// Represents a command to create a new shipping method.
/// </summary>
/// <param name="Name">The shipping method name.</param>
/// <param name="Price">The shipping method price.</param>
/// <param name="EstimatedDeliveryDays">
/// The shipping method estimated delivery days.
/// </param>
[Authorize(roleName: nameof(Role.Admin))]
public record CreateShippingMethodCommand(
    string Name,
    decimal Price,
    int EstimatedDeliveryDays
) : IRequestWithAuthorization<CreatedResult>;
