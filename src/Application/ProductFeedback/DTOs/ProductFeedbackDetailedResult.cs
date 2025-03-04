using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

namespace Application.ProductFeedback.DTOs;

/// <summary>
/// Represents a product feedback result.
/// </summary>
/// <param name="ProductFeedback">The product feedback.</param>
/// <param name="User">The product feedback user.</param>
public record ProductFeedbackDetailedResult(
    DomainProductFeedback ProductFeedback,
    ProductFeedbackUserResult User
) : ProductFeedbackResult(ProductFeedback);
