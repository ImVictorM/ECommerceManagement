using Application.Common.Persistence.Repositories;
using Application.Products.DTOs;
using Application.Products.Errors;

using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Queries.GetProductById;

internal sealed partial class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductPricingService _productPricingService;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        IProductPricingService productPricingService,
        ILogger<GetProductByIdQueryHandler> logger
    )
    {
        _productRepository = productRepository;
        _productPricingService = productPricingService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiateRetrievingProductById(request.Id);

        var productId = ProductId.Create(request.Id);

        var productWithCategories = await _productRepository.GetProductWithCategoriesSatisfyingAsync(
            new QueryActiveProductByIdSpecification(productId),
            cancellationToken
        );

        if (productWithCategories == null)
        {
            LogProductDoesNotExist();
            throw new ProductNotFoundException($"The product with id {productId} does not exist");
        }

        LogProductRetrieved();

        var productPrice = await _productPricingService.CalculateProductPriceApplyingSaleAsync(
            productWithCategories.Product,
            cancellationToken
        );

        LogProductPriceCalculated();

        LogProductRetrievedSuccessfully();

        return new ProductResult(
            productWithCategories.Product,
            productWithCategories.CategoryNames,
            productPrice
        );
    }
}
