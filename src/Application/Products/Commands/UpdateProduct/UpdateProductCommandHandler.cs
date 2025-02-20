using Application.Common.Persistence;
using Application.Products.Errors;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Commands.UpdateProduct;

/// <summary>
/// Handles the <see cref="UpdateProductCommand"/> command.
/// </summary>
public sealed partial class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="logger">The logger.</param>
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

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingProductUpdate(request.Id);

        var productId = ProductId.Create(request.Id);
        var productCategoryIds = request.CategoryIds.Select(CategoryId.Create);

        var productToUpdate = await _productRepository.FindFirstSatisfyingAsync(
            new QueryActiveProductByIdSpecification(productId),
            cancellationToken
        );

        if (productToUpdate == null)
        {
            LogProductDoesNotExist();

            throw new ProductNotFoundException($"The product with id {productId} could not be updated because it does not exist");
        }

        productToUpdate.UpdateProduct(
            request.Name,
            request.Description,
            request.BasePrice,
            request.Images.Select(ProductImage.Create),
            productCategoryIds.Select(ProductCategory.Create)
        );

        await _unitOfWork.SaveChangesAsync();

        LogProductUpdatedSuccessfully();

        return Unit.Value;
    }
}
