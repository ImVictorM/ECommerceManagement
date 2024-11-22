using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace Application.Products.Commands.UpdateProduct;

/// <summary>
/// Handles the <see cref="UpdateProductCommand"/>.
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork"></param>
    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.Create(request.Id);

        var productToUpdate =
            await _unitOfWork.ProductRepository.FindByIdSatisfyingAsync(productId, new QueryProductActiveSpec()) ??
            throw new ProductNotFoundException($"The product with id {productId} could not be updated because it does not exist");

        productToUpdate.UpdateProduct(
            request.Name,
            request.Description,
            request.BasePrice,
            request.Images,
            request.Categories
        );

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
