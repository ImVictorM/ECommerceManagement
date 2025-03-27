using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Products.Errors;

using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Commands.AddStock;

internal sealed partial class AddStockCommandHandler
    : IRequestHandler<AddStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public AddStockCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ILogger<AddStockCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        AddStockCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingAddingStock(request.ProductId);

        var productId = ProductId.Create(request.ProductId);

        var product = await _productRepository.FindFirstSatisfyingAsync(
            new QueryActiveProductByIdSpecification(productId),
            cancellationToken
        );

        if (product == null)
        {
            LogProductDoesNotExist();
            throw new ProductNotFoundException(
                $"It was impossible to add stock for the product with identifier" +
                $" '{productId}' because either the product does not exist or is" +
                $" inactive"
            ) ;
        }

        LogAddingStock(product.Inventory.QuantityAvailable);

        product.Inventory.AddStock(request.QuantityToAdd);

        await _unitOfWork.SaveChangesAsync();

        LogStockAddedSuccessfully(product.Inventory.QuantityAvailable);

        return Unit.Value;
    }
}
