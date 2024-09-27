using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.ProductFeedbackAggregate;

/// <summary>
/// Represents a product feedback.
/// </summary>
public sealed class ProductFeedback : AggregateRoot<ProductFeedbackId>, ISoftDeletable
{
    /// <summary>
    /// Gets the product feedback subject.
    /// </summary>
    public string Subject { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the product feedback content.
    /// </summary>
    public string Content { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the product rating.
    /// </summary>
    public int? StarRating { get; private set; }
    /// <summary>
    /// A boolean indicating if the feedback is active.
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the id of the product the feedback references to.
    /// </summary>
    public ProductId ProductId { get; private set; } = null!;
    /// <summary>
    /// Gets the id of the user who created the feedback.
    /// </summary>
    public UserId UserId { get; private set; } = null!;
    /// <summary>
    /// Gets the id of the order the feedback references to,
    /// </summary>
    public OrderId OrderId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductFeedback"/> class.
    /// </summary>
    private ProductFeedback() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductFeedback"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="productId">The product id.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="subject">The feedback subject.</param>
    /// <param name="content">The feedback text content.</param>
    /// <param name="starRating">The feedback star rating (optional).</param>
    private ProductFeedback(
        UserId userId,
        ProductId productId,
        OrderId orderId,
        string subject,
        string content,
        int? starRating = null
    ) : base(ProductFeedbackId.Create())
    {
        Subject = subject;
        Content = content;
        StarRating = starRating;
        ProductId = productId;
        UserId = userId;
        OrderId = orderId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductFeedback"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="productId">The product id.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="subject">The feedback subject.</param>
    /// <param name="content">The feedback text content.</param>
    /// <param name="starRating">The feedback star rating (optional).</param>
    /// <returns>A new instance of the <see cref="ProductFeedback"/> class.</returns>
    public static ProductFeedback Create(
        UserId userId,
        ProductId productId,
        OrderId orderId,
        string subject,
        string content,
        int? starRating = null
    )
    {
        return new ProductFeedback(
            userId,
            productId,
            orderId,
            subject,
            content,
            starRating
        );
    }

    /// <inheritdoc/>
    public void MakeInactive()
    {
        IsActive = false;
    }
}
