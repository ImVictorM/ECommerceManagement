using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.ProductFeedbackAggregate;

/// <summary>
/// Represents a product feedback.
/// </summary>
public sealed class ProductFeedback : AggregateRoot<ProductFeedbackId>, IActivatable
{
    /// <summary>
    /// Gets the product feedback title.
    /// </summary>
    public string Title { get; private set; } = null!;
    /// <summary>
    /// Gets the product feedback content.
    /// </summary>
    public string Content { get; private set; } = null!;
    /// <summary>
    /// Gets the product star rating.
    /// </summary>
    public StarRating StarRating { get; private set; } = null!;
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

    private ProductFeedback() { }

    private ProductFeedback(
        UserId userId,
        ProductId productId,
        string title,
        string content,
        StarRating starRating
    )
    {
        Title = title;
        Content = content;
        StarRating = starRating;
        ProductId = productId;
        UserId = userId;

        IsActive = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductFeedback"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="productId">The product id.</param>
    /// <param name="title">The feedback title.</param>
    /// <param name="content">The feedback text content.</param>
    /// <param name="starRating">The feedback star rating (optional).</param>
    /// <returns>
    /// A new instance of the <see cref="ProductFeedback"/> class.
    /// </returns>
    public static ProductFeedback Create(
        UserId userId,
        ProductId productId,
        string title,
        string content,
        StarRating starRating
    )
    {
        return new ProductFeedback(
            userId,
            productId,
            title,
            content,
            starRating
        );
    }

    /// <inheritdoc/>
    public void Deactivate()
    {
        IsActive = false;
    }
}
