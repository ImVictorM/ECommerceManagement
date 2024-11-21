using MediatR;

namespace Application.Products.Commands.DeactivateProduct;

/// <summary>
/// Command to deactivate a product.
/// </summary>
/// <param name="Id">The product id.</param>
public record DeactivateProductCommand(string Id) : IRequest<Unit>;
