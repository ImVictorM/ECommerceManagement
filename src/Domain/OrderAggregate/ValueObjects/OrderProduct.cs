using Domain.CategoryAggregate.ValueObjects;
using Domain.OrderAggregate.Interfaces;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents an order product.
/// </summary>
public sealed class OrderProduct : ValueObject, IOrderProductReserved
{
    private readonly HashSet<CategoryId> _productCategoryIds = [];

    /// <inheritdoc/>
    public int Quantity { get; }
    /// <inheritdoc/>
    public ProductId ProductId { get; } = null!;

    /// <summary>
    /// Gets the product base price.
    /// </summary>
    public decimal BasePrice { get; }
    /// <summary>
    /// Gets the product price at purchase.
    /// </summary>
    public decimal PurchasedPrice { get; }
    /// <summary>
    /// Gets the product categories.
    /// </summary>
    public IReadOnlySet<CategoryId> ProductCategoryIds => _productCategoryIds;

    private OrderProduct() { }

    private OrderProduct(
        ProductId productId,
        int quantity,
        decimal basePrice,
        decimal purchasedPrice,
        IReadOnlySet<CategoryId> productCategoryIds
    )
    {
        ProductId = productId;
        Quantity = quantity;
        PurchasedPrice = purchasedPrice;
        BasePrice = basePrice;
        _productCategoryIds.UnionWith(productCategoryIds);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="quantity">The quantity of products ordered.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="purchasedPrice">The product price at purchase.</param>
    /// <param name="productCategoryIds">The product categories.</param>
    /// <returns>A new instance of the <see cref="OrderProduct"/> class.</returns>
    public static OrderProduct Create(
        ProductId productId,
        int quantity,
        decimal basePrice,
        decimal purchasedPrice,
        IReadOnlySet<CategoryId> productCategoryIds
    )
    {
        return new OrderProduct(
            productId,
            quantity,
            basePrice,
            purchasedPrice,
            productCategoryIds
        );
    }

    /// <summary>
    /// Calculates the transaction price, multiplying the purchased price by the quantity.
    /// </summary>
    /// <returns>The transaction price.</returns>
    public decimal CalculateTransactionPrice()
    {
        return PurchasedPrice * Quantity;
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Quantity;
        yield return ProductId;
        yield return PurchasedPrice;
        yield return BasePrice;

        foreach (var category in _productCategoryIds)
        {
            yield return category;
        }
    }
}
