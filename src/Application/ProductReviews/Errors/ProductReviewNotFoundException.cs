using Domain.ProductReviewAggregate.ValueObjects;

using SharedKernel.Errors;

namespace Application.ProductReviews.Errors;

/// <summary>
/// Exception thrown when the requested product review is not found.
/// </summary>
public class ProductReviewNotFoundException : BaseException
{
    private const string DefaultTitle = "Review Not Found";

    private const string DefaultMessage =
        "The requested product review was not found.";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal ProductReviewNotFoundException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ProductReviewNotFoundException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ProductReviewNotFoundException(
        string message,
        Exception innerException
    )
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }

    internal ProductReviewNotFoundException(ProductReviewId reviewId)
        : base(
            $"The product review with identifier '{reviewId}' was not found.",
            DefaultTitle,
            _defaultErrorCode
        )
    {
    }
}
