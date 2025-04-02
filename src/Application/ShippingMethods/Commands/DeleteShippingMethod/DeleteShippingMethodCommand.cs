using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.ShippingMethods.Commands.DeleteShippingMethod;

/// <summary>
/// Represents a command to delete an existent shipping method.
/// </summary>
/// <param name="ShippingMethodId">The shipping method identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeleteShippingMethodCommand(string ShippingMethodId)
    : IRequestWithAuthorization<Unit>;
