using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Interfaces;

namespace Application.Products.Services;

internal sealed class ProductPricingService : IProductPricingService
{
    private readonly ISaleApplicationService _saleApplicationService;
    private readonly IDiscountService _discountService;

    public ProductPricingService(
        ISaleApplicationService saleService,
        IDiscountService discountService
    )
    {
        _saleApplicationService = saleService;
        _discountService = discountService;
    }

    public async Task<decimal> CalculateDiscountedPriceAsync(
        Product product,
        CancellationToken cancellationToken = default
    )
    {
        var result = await CalculateDiscountedPricesAsync([product], cancellationToken);

        return result[product.Id];
    }

    public async Task<Dictionary<ProductId, decimal>> CalculateDiscountedPricesAsync(
        IEnumerable<Product> products,
        CancellationToken cancellationToken = default
    )
    {
        var eligibleProducts = products.Select(p => SaleEligibleProduct.Create(
            p.Id,
            p.ProductCategories.Select(c => c.CategoryId))
        );

        var productSales = await _saleApplicationService.GetApplicableSalesForProductsAsync(
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
