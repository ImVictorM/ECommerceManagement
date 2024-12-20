using Domain.SaleAggregate.ValueObjects;
using SharedKernel.Errors;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.SaleAggregate;

/// <summary>
/// Represents a sale.
/// </summary>
public class Sale : AggregateRoot<SaleId>
{
    private readonly HashSet<SaleCategory> _categoriesInSale = [];
    private readonly HashSet<SaleProduct> _productsInSale = [];
    private readonly HashSet<SaleProduct> _productsExcludeFromSale = [];

    /// <summary>
    /// Gets the sale discount;
    /// </summary>
    public Discount Discount { get; private set; } = null!;
    /// <summary>
    /// Gets the categories in sale.
    /// </summary>
    public IReadOnlySet<SaleCategory> CategoriesInSale => _categoriesInSale;
    /// <summary>
    /// Gets the products in sale.
    /// </summary>
    public IReadOnlySet<SaleProduct> ProductsInSale => _productsInSale;
    /// <summary>
    /// Gets the products excluded from sale.
    /// </summary>
    public IReadOnlySet<SaleProduct> ProductsExcludedFromSale => _productsExcludeFromSale;

    private Sale() { }

    private Sale(
        Discount discount,
        HashSet<SaleCategory> categoriesInSale,
        HashSet<SaleProduct> productsInSale,
        HashSet<SaleProduct> productsExcludeFromSale
    )
    {
        Discount = discount;
        _categoriesInSale = categoriesInSale;
        _productsInSale = productsInSale;
        _productsExcludeFromSale = productsExcludeFromSale;

        ValidateSale();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Sale"/> class.
    /// </summary>
    /// <param name="discount">The sale discount.</param>
    /// <param name="categoriesInSale">The categories in sale.</param>
    /// <param name="productsInSale">The products in sale.</param>
    /// <param name="productsExcludeFromSale">The products excluded from sale.</param>
    /// <returns>A new instance of the <see cref="Sale"/> class.</returns>
    public static Sale Create(
        Discount discount,
        HashSet<SaleCategory> categoriesInSale,
        HashSet<SaleProduct> productsInSale,
        HashSet<SaleProduct> productsExcludeFromSale
    )
    {
        return new Sale(
            discount,
            categoriesInSale,
            productsInSale,
            productsExcludeFromSale
        );
    }

    private void ValidateSale()
    {
        var hasAnyCategory = CategoriesInSale.Any();
        var hasAnyProduct = ProductsInSale.Any();
        var productInSaleAndExcludedProductsAreEqual = ProductsInSale.SetEquals(ProductsExcludedFromSale);

        if ((hasAnyCategory || hasAnyProduct) && !productInSaleAndExcludedProductsAreEqual)
        {
            return;
        }

        throw new DomainValidationException($"A sale must contain at least one category or one product.");
    }
}
