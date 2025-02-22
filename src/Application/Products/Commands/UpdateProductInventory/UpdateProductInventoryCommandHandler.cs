using Application.Common.Persistence;
using Application.Products.Errors;

using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Commands.UpdateProductInventory;

/// <summary>
/// Handler for the <see cref="UpdateProductInventoryCommand"/> command.
/// </summary>
public sealed partial class UpdateProductInventoryCommandHandler : IRequestHandler<UpdateProductInventoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="logger">The logger.</param>
    public UpdateProductInventoryCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ILogger<UpdateProductInventoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductInventoryCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingInventoryUpdate(request.ProductId);

        var productId = ProductId.Create(request.ProductId);

        var product = await _productRepository.FindFirstSatisfyingAsync(
            new QueryActiveProductByIdSpecification(productId),
            cancellationToken
        );

        if (product == null)
        {
            LogProductDoesNotExist();
            throw new ProductNotFoundException($"It was not possible to increment the inventory of the product with id {productId} because the product does not exist");
        }

        LogIncrementingQuantityInInventory(product.Inventory.QuantityAvailable);

        product.Inventory.AddStock(request.QuantityToIncrement);

        await _unitOfWork.SaveChangesAsync();

        LogInventoryUpdatedSuccessfully(product.Inventory.QuantityAvailable);

        return Unit.Value;
    }
}
