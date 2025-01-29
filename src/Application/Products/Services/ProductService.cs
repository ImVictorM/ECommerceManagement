using Application.Common.Persistence;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Services;

namespace Application.Products.Services;

/// <summary>
/// Product services.
/// </summary>
public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleService _saleService;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="saleService">The sale service.</param>
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
    public async Task<decimal> CalculateProductPriceApplyingSaleAsync(Product product)
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
