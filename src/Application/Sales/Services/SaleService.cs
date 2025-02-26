using Application.Common.Persistence.Repositories;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

namespace Application.Sales.Services;

internal sealed class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;

    public SaleService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <inheritdoc/>
    public async Task<IDictionary<ProductId, IEnumerable<Sale>>> GetProductsSalesAsync(
        IEnumerable<SaleProduct> products,
        CancellationToken cancellationToken = default
    )
    {
        var productList = products.ToList();

        var allProductIds = productList.Select(p => p.ProductId).ToHashSet();
        var allCategoryIds = productList.SelectMany(p => p.Categories).ToHashSet();

        var allSales = await _saleRepository.FindAllAsync(sale =>
            sale.ProductsInSale.Any(p => allProductIds.Contains(p.ProductId)) ||
            (sale.CategoriesInSale.Any(c => allCategoryIds.Contains(c.CategoryId)) &&
            !sale.ProductsExcludedFromSale.Any(p => allProductIds.Contains(p.ProductId))),
            cancellationToken
        );

        return productList.ToDictionary(
            product => product.ProductId,
            product => allSales.Where(sale => sale.IsProductInSale(product))
        );
    }
}
