using Application.Common.Persistence;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

namespace Application.Sales.Services;

/// <summary>
/// Services for sales.
/// </summary>
public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="SaleService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public SaleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Sale>> GetProductSalesAsync(SaleProduct product)
    {
        return await _unitOfWork.SaleRepository.FindAllAsync(sale =>
            sale.ProductsInSale.Any(p => p.ProductId == product.ProductId) ||
            (sale.CategoriesInSale.Any(c => product.Categories.Contains(c.CategoryId)) && !sale.ProductsExcludedFromSale.Any(p => p.ProductId == product.ProductId))
         );
    }
}
