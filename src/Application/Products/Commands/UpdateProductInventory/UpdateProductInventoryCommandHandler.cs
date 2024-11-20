using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace Application.Products.Commands.UpdateProductInventory;

/// <summary>
/// Handler for the <see cref="UpdateProductInventoryCommand"/> command.
/// </summary>
public class UpdateProductInventoryCommandHandler : IRequestHandler<UpdateProductInventoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdateProductInventoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductInventoryCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.Create(request.ProductId);

        var product =
            await _unitOfWork.ProductRepository.FindByIdAsync(productId)
            ?? throw new ProductNotFoundException($"It was not possible to increment the inventory of the product with id {productId} because the product does not exist");

        product.IncrementQuantityInInventory(request.QuantityToIncrement);

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
