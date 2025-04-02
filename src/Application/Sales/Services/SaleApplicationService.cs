using Application.Common.Persistence.Repositories;

using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.Specifications;
using Domain.SaleAggregate.ValueObjects;

namespace Application.Sales.Services;

internal sealed class SaleApplicationService : ISaleApplicationService
{
    private readonly ISaleRepository _saleRepository;

    public SaleApplicationService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<IDictionary<
        ProductId,
        IEnumerable<Sale>
    >> GetApplicableSalesForProductsAsync(
        IEnumerable<SaleEligibleProduct> products,
        CancellationToken cancellationToken = default
    )
    {
        var allSales = await _saleRepository.FindSatisfyingAsync(
            new QueryApplicableSalesForProductsSpecification(products),
            cancellationToken
        );

        return products.ToDictionary(
            product => product.ProductId,
            product => allSales.Where(sale => sale.IsProductOnSale(product))
        );
    }
}
