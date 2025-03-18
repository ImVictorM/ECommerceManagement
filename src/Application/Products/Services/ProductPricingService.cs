using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Interfaces;

namespace Application.Products.Services;

internal sealed class ProductPricingService : IProductPricingService
{
    private readonly ISaleApplicationService _saleService;
    private readonly IDiscountService _discountService;

    public ProductPricingService(
        ISaleApplicationService saleService,
        IDiscountService discountService
    )
    {
        _saleService = saleService;
        _discountService = discountService;
    }

    public async Task<decimal> CalculateProductPriceApplyingSaleAsync(
        Product product,
        CancellationToken cancellationToken = default
    )
    {
        var result = await CalculateProductsPriceApplyingSaleAsync([product], cancellationToken);

        return result[product.Id];
    }

    public async Task<Dictionary<ProductId, decimal>> CalculateProductsPriceApplyingSaleAsync(
        IEnumerable<Product> products,
        CancellationToken cancellationToken = default
    )
    {
        var eligibleProducts = products.Select(p => SaleEligibleProduct.Create(
            p.Id,
            p.ProductCategories.Select(c => c.CategoryId))
        );

        var productSales = await _saleService.GetApplicableSalesForProductsAsync(
            eligibleProducts,
            cancellationToken
        );

        return products.ToDictionary(
            p => p.Id,
            p => _discountService.CalculateDiscountedPrice(
                p.BasePrice,
                productSales[p.Id].Select(s => s.Discount)
            )
        );
    }
}
