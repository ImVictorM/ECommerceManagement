using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Products.Commands.AddStock;

/// <summary>
/// Represents a command to increase the stock quantity of a specified product.
/// </summary>
/// <param name="ProductId">The product identifier.</param>
/// <param name="QuantityToAdd">The quantity to add.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record AddStockCommand(
    string ProductId,
    int QuantityToAdd
) : IRequestWithAuthorization<Unit>;
