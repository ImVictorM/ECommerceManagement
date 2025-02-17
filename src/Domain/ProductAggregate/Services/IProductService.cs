namespace Domain.ProductAggregate.Services;

/// <summary>
/// General services for products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves the category names for a product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>A list of category names.</returns>
    Task<IEnumerable<string>> GetProductCategoryNamesAsync(Product product);

    /// <summary>
    /// Calculates the product price applying sales if it has any.
    /// </summary>
    /// <param name="product">The product to calculate the price.</param>
    /// <returns>The product price applying sale.</returns>
    Task<decimal> CalculateProductPriceApplyingSaleAsync(Product product);
}
