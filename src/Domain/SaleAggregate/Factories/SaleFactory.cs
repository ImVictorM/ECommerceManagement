using Domain.SaleAggregate.Errors;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.ValueObjects;

namespace Domain.SaleAggregate.Factories;

/// <summary>
/// Factory responsible for orchestrating the creation of <see cref="Sale"/>
/// objects.
/// </summary>
public sealed class SaleFactory
{
    private readonly ISaleEligibilityService _saleEligibilityService;

    /// <summary>
    /// Initiates a new instance of the <see cref="SaleFactory"/> class.
    /// </summary>
    /// <param name="saleEligibilityService">
    /// The sale eligibility service.
    /// </param>
    public SaleFactory(ISaleEligibilityService saleEligibilityService)
    {
        _saleEligibilityService = saleEligibilityService;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Sale"/> class.
    /// </summary>
    /// <param name="discount">The sale discount.</param>
    /// <param name="categoriesOnSale">The categories on sale.</param>
    /// <param name="productsOnSale">The products on sale.</param>
    /// <param name="productsExcludedFromSale">The products excluded from sale.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A new instance of the <see cref="Sale"/> class.</returns>
    public async Task<Sale> CreateSaleAsync(
        Discount discount,
        IEnumerable<SaleCategory> categoriesOnSale,
        IEnumerable<SaleProduct> productsOnSale,
        IEnumerable<SaleProduct> productsExcludedFromSale,
        CancellationToken cancellationToken = default
    )
    {
        var sale = Sale.Create(
            discount,
            categoriesOnSale,
            productsOnSale,
            productsExcludedFromSale
        );

        var isSaleEligible = await _saleEligibilityService.IsSaleEligibleAsync(
            sale,
            cancellationToken
        );

        if (!isSaleEligible)
        {
            throw new SaleNotEligibleException();
        }

        return sale;
    }
}
