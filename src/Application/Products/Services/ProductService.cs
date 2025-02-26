using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Interfaces;

namespace Application.Products.Services;

internal sealed class ProductService : IProductService
{
    private readonly ISaleService _saleService;
    private readonly IDiscountService _discountService;

    public ProductService(
        ISaleService saleService,
        IDiscountService discountService
    )
    {
        _saleService = saleService;
        _discountService = discountService;
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateProductPriceApplyingSaleAsync(
        Product product,
        CancellationToken cancellationToken = default
    )
    {
        var result = await CalculateProductsPriceApplyingSaleAsync([product], cancellationToken);

        return result[product.Id];
    }

    /// <inheritdoc/>
    public async Task<Dictionary<ProductId, decimal>> CalculateProductsPriceApplyingSaleAsync(
        IEnumerable<Product> products,
        CancellationToken cancellationToken = default
    )
    {
        var saleProducts = products.Select(p => SaleProduct.Create(
            p.Id,
            p.ProductCategories.Select(c => c.CategoryId).ToHashSet())
        );

        var productSales = await _saleService.GetProductsSalesAsync(saleProducts, cancellationToken);

        return products.ToDictionary(
            p => p.Id,
            p => _discountService.CalculateDiscountedPrice(
                p.BasePrice,
                productSales[p.Id]
                    .Where(s => s.IsValidToDate())
                    .Select(s => s.Discount)
                    .ToArray()
            )
        );
    }
}
