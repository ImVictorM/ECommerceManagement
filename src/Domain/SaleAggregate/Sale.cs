using Domain.SaleAggregate.Errors;
using Domain.SaleAggregate.ValueObjects;

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
        IEnumerable<SaleCategory> categoriesInSale,
        IEnumerable<SaleProduct> productsInSale,
        IEnumerable<SaleProduct> productsExcludeFromSale
    )
    {
        Discount = discount;
        _categoriesInSale.UnionWith(categoriesInSale);
        _productsInSale.UnionWith(productsInSale);
        _productsExcludeFromSale.UnionWith(productsExcludeFromSale);

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
        IEnumerable<SaleCategory> categoriesInSale,
        IEnumerable<SaleProduct> productsInSale,
        IEnumerable<SaleProduct> productsExcludeFromSale
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
    /// Checks if a product is on sale.
    /// </summary>
    /// <param name="product">The product to be checked.</param>
    /// <returns>A boolean value indicating if the product is on sale.</returns>
    /// <remarks>
    /// A sale applies to a product if:
    /// - The product is explicitly included in the sale
    /// (exists in <see cref="ProductsInSale"/>).
    /// - OR the product belongs to a category that is on sale
    ///   (exists in <see cref="CategoriesInSale"/>),
    ///   and it is NOT explicitly excluded from the sale
    ///   (exists in <see cref="ProductsExcludedFromSale"/>).
    /// </remarks>
    public bool IsProductOnSale(SaleEligibleProduct product)
    {
        if (!Discount.IsValidToDate)
        {
            return false;
        }

        var currentProduct = SaleProduct.Create(product.ProductId);
        var currentProductCategories = product.CategoryIds.Select(SaleCategory.Create);

        var isProductInSaleList = ProductsInSale.Contains(currentProduct);
        var isProductExcludedFromSale = ProductsExcludedFromSale.Contains(currentProduct);

        var isAnyProductCategoryInSaleList = CategoriesInSale
            .Intersect(currentProductCategories)
            .Any();

        return
            (isProductInSaleList || isAnyProductCategoryInSaleList)
            && !isProductExcludedFromSale;
    }

    private void ValidateSale()
    {
        var hasAnyCategory = CategoriesInSale.Any();
        var hasAnyProduct = ProductsInSale.Any();

        var productInSaleAndExcludedProductsAreEqual = ProductsInSale
            .SetEquals(ProductsExcludedFromSale);

        if (
            hasAnyCategory
            || (hasAnyProduct && !productInSaleAndExcludedProductsAreEqual))
        {
            return;
        }

        throw new InvalidSaleStateException(
            "A sale must contain at least one category or one product"
        );
    }
}
