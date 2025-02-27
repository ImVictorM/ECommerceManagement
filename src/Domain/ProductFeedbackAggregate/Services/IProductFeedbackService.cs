using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.ProductFeedbackAggregate.Services;

/// <summary>
/// Represents a contract for product feedback services.
/// </summary>
public interface IProductFeedbackService
{
    /// <summary>
    /// Verifies if a user can leave a feedback for the specified
    /// product.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="productId">The product id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A boolean value indicating if the user can leave a
    /// feedback for the specified product.
    /// </returns>
    Task<bool> CanLeaveFeedbackAsync(
        UserId userId,
        ProductId productId,
        CancellationToken cancellationToken = default
    );
}
