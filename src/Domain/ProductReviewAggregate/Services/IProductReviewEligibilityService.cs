using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.ProductReviewAggregate.Services;

/// <summary>
/// Represents a contract for verifying product review eligibility.
/// </summary>
public interface IProductReviewEligibilityService
{
    /// <summary>
    /// Determines whether a user is eligible to leave a review for the
    /// specified product.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="productId">The product identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A boolean value indicating whether the user is eligible to leave a
    /// review for the specified product.
    /// </returns>
    Task<bool> CanLeaveReviewAsync(
        UserId userId,
        ProductId productId,
        CancellationToken cancellationToken = default
    );
}
