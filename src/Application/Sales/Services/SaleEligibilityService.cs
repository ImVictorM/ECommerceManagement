using Application.Common.Persistence.Repositories;
using Application.Sales.Errors;

using Domain.ProductAggregate.Specifications;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Application.Sales.Services;

internal class SaleEligibilityService : ISaleEligibilityService
{
    private readonly ISaleApplicationService _saleApplicationService;
    private readonly IProductRepository _productRepository;
    private readonly IDiscountService _discountService;

    public SaleEligibilityService(
        ISaleApplicationService saleApplicationService,
        IProductRepository productRepository,
        IDiscountService discountService
    )
    {
        _saleApplicationService = saleApplicationService;
        _productRepository = productRepository;
        _discountService = discountService;
    }

    public async Task EnsureSaleProductsEligibilityAsync(
        Sale sale,
        CancellationToken cancellationToken
    )
    {
        var productOnSaleIds = sale.ProductsOnSale.Select(p => p.ProductId);

        var products = await _productRepository.FindSatisfyingAsync(
            new QueryProductsContainingIdsSpecification(productOnSaleIds),
            cancellationToken
        );

        var eligibleProducts = products.Select(p => SaleEligibleProduct.Create(
            p.Id,
            p.ProductCategories.Select(c => c.CategoryId)
        ));

        var productsWithSalesMap = await _saleApplicationService
            .GetApplicableSalesForProductsAsync(eligibleProducts, cancellationToken);

        foreach (var product in products)
        {
            productsWithSalesMap.TryGetValue(
                product.Id,
                out var currentProductSales
            );

            List<Discount> newProductDiscounts =
            [
                .. currentProductSales?.Select(s => s.Discount),
                sale.Discount
            ];

            var newPriceOnSale = _discountService.CalculateDiscountedPrice(
                product.BasePrice,
                newProductDiscounts
            );

            var thresholdSpecification =
                new ProductSaleDiscountThresholdSpecification(newPriceOnSale);

            if (!thresholdSpecification.IsSatisfiedBy(product))
            {
                throw new SaleProductNotEligibleException(product.Id);
            }
        }
    }
}
