using Application.Common.DTOs;
using Application.Common.Persistence;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Commands.CreateProduct;

/// <summary>
/// Handles product creation.
/// </summary>
public sealed partial class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="logger">The logger.</param>
    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ILogger<CreateProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingProductCreation(request.Name);

        var categoryIds = request.CategoryIds.Select(CategoryId.Create);

        var newProduct = Product.Create(
            request.Name,
            request.Description,
            request.BasePrice,
            request.InitialQuantity,
            categoryIds.Select(ProductCategory.Create),
            request.Images.Select(ProductImage.Create)
        );

        LogProductCreatedInitiatingPersistence();

        await _productRepository.AddAsync(newProduct);

        await _unitOfWork.SaveChangesAsync();

        LogProductPersistedSuccessfully(newProduct.Id.ToString());

        return new CreatedResult(newProduct.Id.ToString());
    }
}
