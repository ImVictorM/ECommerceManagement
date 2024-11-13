using Application.Common.DTOs;
using MediatR;
using SharedKernel.ValueObjects;

namespace Application.Products.Commands.CreateProduct;

/// <summary>
/// Represents a command to create a new product.
/// </summary>
/// <param name="Name">The new product name.</param>
/// <param name="Description">The new product description.</param>
/// <param name="InitialQuantity">The new product initial quantity to be placed in the product's inventory.</param>
/// <param name="InitialPrice">The new product initial price.</param>
/// <param name="Categories">Categories the new product belongs to.</param>
/// <param name="Images">The new product images.</param>
/// <param name="InitialDiscounts">The new product initial discounts (optional).</param>
public record CreateProductCommand(
    string Name,
    string Description,
    int InitialQuantity,
    decimal InitialPrice,
    IEnumerable<string> Categories,
    IEnumerable<Uri> Images,
    IEnumerable<Discount>? InitialDiscounts = null
) : IRequest<CreatedResult>;
