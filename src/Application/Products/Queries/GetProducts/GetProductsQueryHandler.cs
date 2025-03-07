using Application.Common.Persistence.Repositories;
using Application.Products.DTOs;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Queries.GetProducts;

internal sealed partial class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IEnumerable<ProductResult>>
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

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        LogInitiatedRetrievingProducts();

        var page = request.Page ?? DefaultInitialPage;
        var pageSize = request.PageSize ?? DefaultPageSize;
        var categoriesToFilter = request.Categories == null || !request.Categories.Any()
            ? "none" : string.Join(',', request.Categories);

        LogPaginationDetails(page, pageSize);
        LogCategoriesFilterDetails(categoriesToFilter);

        CompositeQuerySpecification<Product> spec = new QueryActiveProductSpecification();

        if (request.Categories != null && request.Categories.Any())
        {
            var categoryIds = request.Categories.Select(CategoryId.Create);

            spec = spec.And(new QueryProductsContainingCategoriesSpecification(categoryIds));
        }

        var productsWithCategories = await _productRepository.GetProductsWithCategoriesSatisfyingAsync(
            spec,
            page,
            pageSize,
            cancellationToken
        );

        LogProductsRetrieved(productsWithCategories.Count());

        var productPricesOnSale = await _productPricingService.CalculateProductsPriceApplyingSaleAsync(
            productsWithCategories.Select(p => p.Product),
            cancellationToken
        );

        LogProductsPriceCalculated();

        LogProductsRetrievedSuccessfully();

        return productsWithCategories.Select(p => new ProductResult(
            p.Product,
            p.CategoryNames,
            productPricesOnSale[p.Product.Id]
        ));
    }
}
