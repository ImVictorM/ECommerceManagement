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
    private HashSet<SaleCategory> _categoriesOnSale = [];
    private HashSet<SaleProduct> _productsOnSale = [];
    private HashSet<SaleProduct> _productsExcludeFromSale = [];

    /// <summary>
    /// Gets the sale discount;
    /// </summary>
    public Discount Discount { get; private set; } = null!;
    /// <summary>
    /// Gets the categories on sale.
    /// </summary>
    public IReadOnlySet<SaleCategory> CategoriesOnSale => _categoriesOnSale;
    /// <summary>
    /// Gets the products on sale.
    /// </summary>
    public IReadOnlySet<SaleProduct> ProductsOnSale => _productsOnSale;
    /// <summary>
    /// Gets the products excluded from sale.
    /// </summary>
    public IReadOnlySet<SaleProduct> ProductsExcludedFromSale => _productsExcludeFromSale;

    private Sale() { }

    private Sale(
        Discount discount,
        IEnumerable<SaleCategory> categoriesOnSale,
        IEnumerable<SaleProduct> productsOnSale,
        IEnumerable<SaleProduct> productsExcludedFromSale
    )
    {
        Discount = discount;

        _categoriesOnSale = categoriesOnSale.ToHashSet();
        _productsOnSale = productsOnSale.ToHashSet();
        _productsExcludeFromSale = productsExcludedFromSale.ToHashSet();

        ValidateSale();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Sale"/> class.
    /// </summary>
    /// <param name="discount">The sale discount.</param>
    /// <param name="categoriesOnSale">The categories on sale.</param>
    /// <param name="productsOnSale">The products on sale.</param>
    /// <param name="productsExcludedFromSale">
    /// The products excluded from sale.
    /// </param>
    /// <returns>A new instance of the <see cref="Sale"/> class.</returns>
    public static Sale Create(
        Discount discount,
        IEnumerable<SaleCategory> categoriesOnSale,
        IEnumerable<SaleProduct> productsOnSale,
        IEnumerable<SaleProduct> productsExcludedFromSale
    )
    {
        return new Sale(
            discount,
            categoriesOnSale,
            productsOnSale,
            productsExcludedFromSale
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
    /// (exists in <see cref="ProductsOnSale"/>).
    /// - OR the product belongs to a category that is on sale
    ///   (exists in <see cref="CategoriesOnSale"/>),
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

        var isProductOnSale = ProductsOnSale.Contains(currentProduct);
        var isProductExcludedFromSale = ProductsExcludedFromSale.Contains(currentProduct);

        var isAnyProductCategoryOnSale = CategoriesOnSale
            .Intersect(currentProductCategories)
            .Any();

        return
            (isProductOnSale || isAnyProductCategoryOnSale)
            && !isProductExcludedFromSale;
    }

    /// <summary>
    /// Updates the current sale.
    /// </summary>
    /// <param name="discount">The new sale discount.</param>
    /// <param name="categoriesOnSale">The new categories on sale.</param>
    /// <param name="productsOnSale">The new products on sale.</param>
    /// <param name="productsExcludedFromSale">
    /// The new products excluded from sale.
    /// </param>
    public void Update(
        Discount discount,
        IEnumerable<SaleCategory> categoriesOnSale,
        IEnumerable<SaleProduct> productsOnSale,
        IEnumerable<SaleProduct> productsExcludedFromSale
    )
    {
        Discount = discount;

        _categoriesOnSale = categoriesOnSale.ToHashSet();
        _productsOnSale = productsOnSale.ToHashSet();
        _productsExcludeFromSale = productsExcludedFromSale.ToHashSet();

        ValidateSale();
    }

    private void ValidateSale()
    {
        var hasAnyCategory = CategoriesOnSale.Any();
        var hasAnyProduct = ProductsOnSale.Any();

        var productOnSaleAndProductsExcludedAreEqual = ProductsOnSale
            .SetEquals(ProductsExcludedFromSale);

        if (
            hasAnyCategory
            || (hasAnyProduct && !productOnSaleAndProductsExcludedAreEqual))
        {
            return;
        }

        throw new InvalidSaleStateException();
    }
}
