using Application.Common.Security.Authorization.Requests;

using MediatR;
using SharedKernel.ValueObjects;

namespace Application.Products.Commands.DeactivateProduct;

/// <summary>
/// Command to deactivate a product.
/// </summary>
/// <param name="Id">The product id.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeactivateProductCommand(string Id) : IRequestWithAuthorization<Unit>;
