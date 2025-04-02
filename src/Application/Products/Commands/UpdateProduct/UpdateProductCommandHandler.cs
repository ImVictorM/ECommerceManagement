using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Products.Errors;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Commands.UpdateProduct;

internal sealed partial class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ILogger<UpdateProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingProductUpdate(request.Id);

        var productId = ProductId.Create(request.Id);

        var productImages = request.Images
            .Select(ProductImage.Create);

        var productCategories = request.CategoryIds
            .Select(CategoryId.Create)
            .Select(ProductCategory.Create);

        var productToUpdate = await _productRepository.FindFirstSatisfyingAsync(
            new QueryActiveProductByIdSpecification(productId),
            cancellationToken
        );

        if (productToUpdate == null)
        {
            LogProductDoesNotExist();

            throw new ProductNotFoundException(
                $"The product with id {productId} could not be updated because" +
                $" it does not exist"
            );
        }

        productToUpdate.Update(
            request.Name,
            request.Description,
            request.BasePrice,
            productImages,
            productCategories
        );

        await _unitOfWork.SaveChangesAsync();

        LogProductUpdatedSuccessfully();

        return Unit.Value;
    }
}
