using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

namespace Application.ProductFeedback.DTOs;

/// <summary>
/// Represents product feedback result.
/// </summary>
/// <param name="ProductFeedback">The product feedback.</param>
public record ProductFeedbackResult(DomainProductFeedback ProductFeedback);
