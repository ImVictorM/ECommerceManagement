using Application.Common.Persistence.Repositories;
using Application.Products.DTOs.Results;
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

    public async Task<ProductResult> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingProductRetrieval(request.Id);

        var productId = ProductId.Create(request.Id);

        var product = await _productRepository.GetProductSatisfyingAsync(
            new QueryActiveProductByIdSpecification(productId),
            cancellationToken
        );

        if (product == null)
        {
            LogProductDoesNotExist();

            throw new ProductNotFoundException()
                .WithContext("ProductId", productId.ToString());
        }

        LogProductRetrieved();

        var productPrice = await _productPricingService.CalculateDiscountedPriceAsync(
            product,
            cancellationToken
        );

        LogProductPriceCalculated();

        LogProductRetrievedSuccessfully();

        return ProductResult.FromProductWithDiscountedPrice(
            product,
            productPrice
        );
    }
}
