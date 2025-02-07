using Application.Common.Security.Authorization.Requests;

using MediatR;
using SharedKernel.ValueObjects;

namespace Application.Products.Commands.UpdateProductInventory;

/// <summary>
/// Represents a command to update a product's inventory.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="QuantityToIncrement">The quantity to add to the inventory.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record UpdateProductInventoryCommand(string ProductId, int QuantityToIncrement) : IRequestWithAuthorization<Unit>;
