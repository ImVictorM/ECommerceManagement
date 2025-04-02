using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Products.Commands.DeactivateProduct;

/// <summary>
/// Represents a command to deactivate a product.
/// </summary>
/// <param name="Id">The product identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeactivateProductCommand(string Id)
    : IRequestWithAuthorization<Unit>;
