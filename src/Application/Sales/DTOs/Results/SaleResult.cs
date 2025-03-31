using Domain.SaleAggregate;

using SharedKernel.ValueObjects;

namespace Application.Sales.DTOs.Results;

/// <summary>
/// Represents a sale result.
/// </summary>
public class SaleResult
{
    /// <summary>
    /// Gets the sale identifier.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the sale discount.
    /// </summary>
    public Discount Discount { get; }
    /// <summary>
    /// Gets the category on sale identifiers.
    /// </summary>
    public IReadOnlyList<string> CategoryOnSaleIds { get; }
    /// <summary>
    /// Gets the product on sale identifiers.
    /// </summary>
    public IReadOnlyList<string> ProductOnSaleIds { get; }
    /// <summary>
    /// Gets the product excluded from sale identifiers.
    /// </summary>
    public IReadOnlyList<string> ProductExcludedFromSaleIds { get; }

    private SaleResult(Sale sale)
    {
        Id = sale.Id.ToString();
        Discount = sale.Discount;

        CategoryOnSaleIds = sale.CategoriesOnSale
            .Select(c => c.CategoryId.ToString())
            .ToList();
        ProductOnSaleIds = sale.ProductsOnSale
            .Select(p => p.ProductId.ToString())
            .ToList();
        ProductExcludedFromSaleIds = sale.ProductsExcludedFromSale
            .Select(p => p.ProductId.ToString())
            .ToList();
    }

    internal static SaleResult FromSale(Sale sale)
    {
        return new SaleResult(sale);
    }
}
