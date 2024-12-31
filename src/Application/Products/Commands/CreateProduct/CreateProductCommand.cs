using Application.Common.DTOs;
using MediatR;

namespace Application.Products.Commands.CreateProduct;

/// <summary>
/// Represents a command to create a new product.
/// </summary>
/// <param name="Name">The new product name.</param>
/// <param name="Description">The new product description.</param>
/// <param name="InitialQuantity">The new product initial quantity to be placed in the product's inventory.</param>
/// <param name="BasePrice">The new product base price.</param>
/// <param name="CategoryIds">Categories the new product belongs to.</param>
/// <param name="Images">The new product images.</param>
public record CreateProductCommand(
    string Name,
    string Description,
    int InitialQuantity,
    decimal BasePrice,
    IEnumerable<string> CategoryIds,
    IEnumerable<Uri> Images
) : IRequest<CreatedResult>;
