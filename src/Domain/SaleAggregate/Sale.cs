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
    private readonly HashSet<CategoryReference> _categoriesInSale = [];
    private readonly HashSet<SaleReference> _productsInSale = [];
    private readonly HashSet<SaleReference> _productsExcludeFromSale = [];

    /// <summary>
    /// Gets the sale discount;
    /// </summary>
    public Discount Discount { get; private set; } = null!;
    /// <summary>
    /// Gets the categories in sale.
    /// </summary>
    public IReadOnlySet<CategoryReference> CategoriesInSale => _categoriesInSale;
    /// <summary>
    /// Gets the products in sale.
    /// </summary>
    public IReadOnlySet<SaleReference> ProductsInSale => _productsInSale;
    /// <summary>
    /// Gets the products excluded from sale.
    /// </summary>
    public IReadOnlySet<SaleReference> ProductsExcludedFromSale => _productsExcludeFromSale;

    private Sale() { }

    private Sale(
        Discount discount,
        HashSet<CategoryReference> categoriesInSale,
        HashSet<SaleReference> productsInSale,
        HashSet<SaleReference> productsExcludeFromSale
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
        HashSet<CategoryReference> categoriesInSale,
        HashSet<SaleReference> productsInSale,
        HashSet<SaleReference> productsExcludeFromSale
    )
    {
        return new Sale(
            discount,
            categoriesInSale,
            productsInSale,
            productsExcludeFromSale
        );
    }

    /// <summary>
    /// Checks if the sale is valid to the current date.
    /// </summary>
    /// <returns>A boolean value Indicates if the sale is valid to the current date.</returns>
    public bool IsValidToDate()
    {
        return Discount.IsValidToDate;
    }

    /// <summary>
    /// Checks if a product is in sale.
    /// </summary>
    /// <param name="product">The product to be checked.</param>
    /// <returns>A boolean value indicating if the product is in sale.</returns>
    public bool IsProductInSale(SaleProduct product)
    {
        var isProductInSaleList = ProductsInSale.Contains(SaleReference.Create(product.ProductId));

        var isAnyProductCategoryInSaleList = CategoriesInSale.Intersect(product.Categories.Select(CategoryReference.Create)).Any();

        return isProductInSaleList || isAnyProductCategoryInSaleList;
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
