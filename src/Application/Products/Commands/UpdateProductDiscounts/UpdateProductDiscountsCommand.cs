using MediatR;
using SharedKernel.ValueObjects;

namespace Application.Products.Commands.UpdateProductDiscounts;

/// <summary>
/// Command to update a product's discount list.
/// </summary>
/// <param name="Id">The product id.</param>
/// <param name="Discounts">The new discounts.</param>
public record UpdateProductDiscountsCommand(string Id, IEnumerable<Discount> Discounts) : IRequest<Unit>;
