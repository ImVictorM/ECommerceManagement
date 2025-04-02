using Application.Common.Persistence.Repositories;
using Application.Common.DTOs.Pagination;
using Application.Products.DTOs.Results;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Queries.GetProducts;

internal sealed partial class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductPricingService _productPricingService;
    private const int DefaultInitialPage = 1;
    private const int DefaultPageSize = 20;

    public GetProductsQueryHandler(
        IProductRepository productRepository,
        IProductPricingService productPricingService,
        ILogger<GetProductsQueryHandler> logger
    )
    {
        _productPricingService = productPricingService;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductResult>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatedRetrievingProducts();

        var page = request.PaginationParams.Page ?? DefaultInitialPage;
        var pageSize = request.PaginationParams.PageSize ?? DefaultPageSize;

        var hasAnyCategoryFilter =
            request.Filters.CategoryIds != null
            && request.Filters.CategoryIds.Any();

        var categoriesToFilter = hasAnyCategoryFilter
            ? string.Join(',', request.Filters.CategoryIds!)
            : "none";

        LogPaginationDetails(page, pageSize);
        LogCategoriesFilterDetails(categoriesToFilter);

        CompositeQuerySpecification<Product> spec = new QueryActiveProductSpecification();

        if (hasAnyCategoryFilter)
        {
            var categoryIds = request.Filters.CategoryIds!.Select(CategoryId.Create);

            spec = spec.And(
                new QueryProductsContainingCategoriesSpecification(categoryIds)
            );
        }

        var products = await _productRepository.GetProductsSatisfyingAsync(
            spec,
            new PaginationParams(page, pageSize),
            cancellationToken
        );

        LogProductsRetrieved(products.Count);

        var productDiscountedPrices = await _productPricingService
            .CalculateDiscountedPricesAsync(products, cancellationToken);

        LogProductsDiscountedPriceCalculated();

        LogProductsRetrievedSuccessfully();

        return products
            .Select(product => ProductResult.FromProductWithDiscountedPrice(
                product,
                productDiscountedPrices[product.Id]
            ))
            .ToList();
    }
}
