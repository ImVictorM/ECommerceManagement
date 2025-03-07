using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents the initial draft of an order line item.
/// </summary>
public sealed class OrderLineItemDraft : ValueObject
{
    /// <summary>
    /// Gets the identifier of the product being ordered.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    /// <summary>
    /// Gets the quantity of the product that has been ordered.
    /// </summary>
    public int Quantity { get; }

    private OrderLineItemDraft() { }

    private OrderLineItemDraft(ProductId productId, int quantityOrdered)
    {
        ProductId = productId;
        Quantity = quantityOrdered;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderLineItemDraft"/> class.
    /// </summary>
    /// <param name="productId">The identifier of the product to be ordered.</param>
    /// <param name="quantity">The quantity ordered.</param>
    /// <returns>
    /// A new instance of <see cref="OrderLineItemDraft"/>.
    /// </returns>
    public static OrderLineItemDraft OrdererProduct(ProductId productId, int quantity)
    {
        return new OrderLineItemDraft(productId, quantity);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
        yield return Quantity;
    }
}
