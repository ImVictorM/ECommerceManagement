using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProduct;

/// <summary>
/// Handles the <see cref="UpdateProductCommand"/>.
/// </summary>
public sealed partial class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingProductUpdate(request.Id);

        var productId = ProductId.Create(request.Id);

        var productToUpdate = await _unitOfWork.ProductRepository.FindByIdSatisfyingAsync(productId, new QueryProductActiveSpec());

        if (productToUpdate == null)
        {
            LogProductDoesNotExist();

            throw new ProductNotFoundException($"The product with id {productId} could not be updated because it does not exist");
        }
            

        productToUpdate.UpdateProduct(
            request.Name,
            request.Description,
            request.BasePrice,
            request.Images,
            request.Categories
        );

        LogProductUpdatedSuccessfully();

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
