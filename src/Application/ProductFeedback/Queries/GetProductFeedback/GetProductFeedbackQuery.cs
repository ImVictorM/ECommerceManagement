using Application.ProductFeedback.DTOs;

using MediatR;

namespace Application.ProductFeedback.Queries.GetProductFeedback;

/// <summary>
/// Represents a query to retrieve active feedback for the specified
/// product.
/// </summary>
/// <param name="ProductId">The product id.</param>
public record GetProductFeedbackQuery(string ProductId)
    : IRequest<IEnumerable<ProductFeedbackResult>>;
