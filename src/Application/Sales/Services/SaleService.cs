using Application.Common.Interfaces.Persistence;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

namespace Application.Sales.Services;

internal class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;

    public SaleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Sale>> GetProductSalesAsync(SaleProduct product)
    {
        return await _unitOfWork.SaleRepository.FindAllAsync(sale => sale.IsProductInSale(product));
    }
}
