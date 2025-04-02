using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate.Services;

/// <summary>
/// Provides pricing services for products.
/// </summary>
public interface IProductPricingService
{
    /// <summary>
    /// Calculates the final price of a product after applying any applicable
    /// sales discounts.
    /// </summary>
    /// <param name="product">
    /// The product for which the price is calculated.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The final price of the product after applying any sales discounts.
    /// </returns>
    Task<decimal> CalculateDiscountedPriceAsync(
        Product product,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Calculates the final prices for multiple products after applying applicable
    /// sales discounts.
    /// </summary>
    /// <param name="products">
    /// The collection of products to calculate prices for.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A dictionary where the keys are product ids and values are their respective
    /// final prices.
    /// </returns>
    Task<Dictionary<ProductId, decimal>> CalculateDiscountedPricesAsync(
        IEnumerable<Product> products,
        CancellationToken cancellationToken = default
    );
}
