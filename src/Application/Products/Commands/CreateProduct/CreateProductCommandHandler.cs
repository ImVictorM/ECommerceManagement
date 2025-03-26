using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.Common.DTOs.Results;

namespace Application.Products.Commands.CreateProduct;

internal sealed partial class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

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

    public async Task<CreatedResult> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
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

        var productId = newProduct.Id.ToString();

        LogProductPersistedSuccessfully(productId);

        return new CreatedResult(productId);
    }
}
