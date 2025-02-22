using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate.Services;

/// <summary>
/// General services for products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Calculates the product price applying sales if it has any.
    /// </summary>
    /// <param name="product">The product to calculate the price.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The product price applying sale.</returns>
    Task<decimal> CalculateProductPriceApplyingSaleAsync(
        Product product,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Calculates the price applying sales to a given collection of products.
    /// </summary>
    /// <param name="products">The products to be calculated the price on sale.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dictionary where each <see cref="ProductId"/> key maps to the product price on sale.</returns>
    Task<Dictionary<ProductId, decimal>> CalculateProductsPriceApplyingSaleAsync(
        IEnumerable<Product> products,
        CancellationToken cancellationToken = default
    );
}
