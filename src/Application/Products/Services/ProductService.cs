using Application.Common.Interfaces.Persistence;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Services;

namespace Application.Products.Services;

internal class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleService _saleService;

    public ProductService(IUnitOfWork unitOfWork, ISaleService saleService)
    {
        _unitOfWork = unitOfWork;
        _saleService = saleService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetProductCategoryNamesAsync(Product product)
    {
        var productCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId);

        var productCategories = await _unitOfWork.CategoryRepository.FindAllAsync(c => productCategoryIds.Contains(c.Id));

        return productCategories.Select(pc => pc.Name);
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateProductPriceAfterSaleAsync(Product product)
    {
        var productCategoriesIds = product.ProductCategories.Select(c => c.CategoryId).ToHashSet();

        var productSales = await _saleService.GetProductSalesAsync(SaleProduct.Create(product.Id, productCategoriesIds));

        var discountsValid = productSales
            .Where(sale => sale.IsValidToDate())
            .Select(sale => sale.Discount)
            .ToArray();

        return DiscountService.ApplyDiscounts(product.BasePrice, discountsValid);
    }
}
