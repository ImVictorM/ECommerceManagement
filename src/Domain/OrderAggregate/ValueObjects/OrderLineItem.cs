using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents a fully assembled order line item.
/// </summary>
public sealed class OrderLineItem : ValueObject
{
    private readonly HashSet<CategoryId> _productCategoryIds = [];

    /// <summary>
    /// Gets the identifier of the product associated with this order line item.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    /// <summary>
    /// Gets the quantity of the product ordered.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the base price of the product at the time of ordering.
    /// </summary>
    public decimal BasePrice { get; }

    /// <summary>
    /// Gets the final price paid for the product, after applying sales.
    /// </summary>
    public decimal PurchasedPrice { get; }

    /// <summary>
    /// Gets the collection of category identifiers to which the product belongs.
    /// </summary>
    public IReadOnlySet<CategoryId> ProductCategoryIds => _productCategoryIds;

    private OrderLineItem() { }

    private OrderLineItem(
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
    /// Creates a new instance of the <see cref="OrderLineItem"/> class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantity">The quantity of the product ordered.</param>
    /// <param name="basePrice">The base price of the product.</param>
    /// <param name="purchasedPrice">The final price paid for the product.</param>
    /// <param name="productCategoryIds">
    /// The identifiers of the product categories.
    /// </param>
    /// <returns>A new instance of the<see cref="OrderLineItem"/> class.</returns>
    public static OrderLineItem Create(
        ProductId productId,
        int quantity,
        decimal basePrice,
        decimal purchasedPrice,
        IReadOnlySet<CategoryId> productCategoryIds
    )
    {
        return new OrderLineItem(
            productId,
            quantity,
            basePrice,
            purchasedPrice,
            productCategoryIds
        );
    }

    /// <summary>
    /// Calculates the total transaction price for this line item,
    /// by multiplying the purchased price by the quantity.
    /// </summary>
    /// <returns>The calculated transaction price.</returns>
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

        foreach (var categoryId in _productCategoryIds)
        {
            yield return categoryId;
        }
    }
}
