using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.DTOs;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;

using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Models;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Handles the <see cref="GetProductsQuery"/> query.
/// </summary>
public sealed partial class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;
    private const int DefaultProductQuantityToTake = 20;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductsQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productService">The product service.</param>
    /// <param name="logger">The logger.</param>
    public GetProductsQueryHandler(
        IUnitOfWork unitOfWork,
        IProductService productService,
        ILogger<GetProductsQueryHandler> logger
    )
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        LogInitiatedRetrievingProducts();
        var limit = request.Limit ?? DefaultProductQuantityToTake;

        CompositeQuerySpecification<Product> spec = new QueryActiveProductSpecification();

        if (request.Categories != null && request.Categories.Any())
        {
            var categoryIds = request.Categories.Select(CategoryId.Create);

            spec = spec.And(new QueryProductsContainingCategoriesSpecification(categoryIds));
        }

        var products = await _unitOfWork.ProductRepository.FindSatisfyingAsync(spec, limit: limit);

        LogProductsRetrievedSuccessfully(limit, request.Categories != null ? string.Join(',', request.Categories) : "none");

        List<ProductResult> productResults = [];

        foreach (var product in products)
        {
            var productPrice = await _productService.CalculateProductPriceApplyingSaleAsync(product);
            var productCategories = await _productService.GetProductCategoryNamesAsync(product);

            productResults.Add(new ProductResult(product, productCategories, productPrice));
        }

        return productResults;
    }
}
