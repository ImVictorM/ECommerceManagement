using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProductInventory;

/// <summary>
/// Handler for the <see cref="UpdateProductInventoryCommand"/> command.
/// </summary>
public sealed partial class UpdateProductInventoryCommandHandler : IRequestHandler<UpdateProductInventoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateProductInventoryCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateProductInventoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductInventoryCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingInventoryUpdate(request.ProductId);

        var productId = ProductId.Create(request.ProductId);

        var product = await _unitOfWork.ProductRepository.FindByIdSatisfyingAsync(productId, new QueryProductActiveSpec());

        if (product == null)
        {
            LogProductDoesNotExist();
            throw new ProductNotFoundException($"It was not possible to increment the inventory of the product with id {productId} because the product does not exist");
        }

        LogIncrementingQuantityInInventory(product.Inventory.QuantityAvailable);

        product.IncrementQuantityInInventory(request.QuantityToIncrement);

        await _unitOfWork.SaveChangesAsync();

        LogInventoryUpdatedSuccessfully(product.Inventory.QuantityAvailable);

        return Unit.Value;
    }
}
