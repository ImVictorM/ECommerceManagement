using Domain.OrderAggregate.ValueObjects;

namespace Domain.OrderAggregate.Services;

/// <summary>
/// Defines the contract for a service that handles order assembly.
/// </summary>
public interface IOrderAssemblyService
{
    /// <summary>
    /// Assembles and enriches order line items from their draft representations.
    /// </summary>
    /// <param name="lineItemDrafts">
    /// The collection of raw order line item drafts.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A collection of <see cref="OrderLineItem"/> instances.
    /// </returns>
    Task<IEnumerable<OrderLineItem>> AssembleOrderLineItemsAsync(
        IEnumerable<OrderLineItemDraft> lineItemDrafts,
        CancellationToken cancellationToken = default
    );
}
