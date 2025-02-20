using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Services;

namespace Application.Products.Services;

/// <summary>
/// Product services.
/// </summary>
public class ProductService : IProductService
{
    private readonly ISaleService _saleService;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="saleService">The sale service.</param>
    public ProductService(ISaleService saleService)
    {
        _saleService = saleService;
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
            p => DiscountService.ApplyDiscounts(
                p.BasePrice,
                productSales[p.Id]
                    .Where(s => s.IsValidToDate())
                    .Select(s => s.Discount)
                    .ToArray()
            )
        );
    }
}
