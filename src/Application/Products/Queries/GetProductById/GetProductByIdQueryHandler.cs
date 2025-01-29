using Application.Common.Persistence;
using Application.Products.DTOs;
using Application.Products.Errors;

using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductById;

/// <summary>
/// Query handler for the <see cref="GetProductByIdQuery"/> query;
/// </summary>
public sealed partial class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productService">The product service.</param>
    /// <param name="logger">The logger.</param>
    public GetProductByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IProductService productService,
        ILogger<GetProductByIdQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiateRetrievingProductById(request.Id);

        var productId = ProductId.Create(request.Id);

        var product = await _unitOfWork.ProductRepository.FindFirstSatisfyingAsync(new QueryActiveProductByIdSpecification(productId));

        if (product == null)
        {
            LogProductDoesNotExist();
            throw new ProductNotFoundException($"The product with id {productId} does not exist");
        }

        LogProductFound();

        LogCalculatingProductCurrentPrice();
        var productPrice = await _productService.CalculateProductPriceApplyingSaleAsync(product);

        LogGettingProductCategories();
        var productCategories = await _productService.GetProductCategoryNamesAsync(product);

        return new ProductResult(
            product,
            productCategories,
            productPrice
        );
    }
}
