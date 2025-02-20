using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Products.Commands.UpdateProduct;

/// <summary>
/// Represents a command to update a product.
/// </summary>
/// <param name="Id">The product id.</param>
/// <param name="Name">The new product name.</param>
/// <param name="Description">The new product description.</param>
/// <param name="BasePrice">The new product base price.</param>
/// <param name="Images">The new product images.</param>
/// <param name="CategoryIds">The product new categories ids.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record UpdateProductCommand(
    string Id,
    string Name,
    string Description,
    decimal BasePrice,
    IEnumerable<Uri> Images,
    IEnumerable<string> CategoryIds
) : IRequestWithAuthorization<Unit>;
