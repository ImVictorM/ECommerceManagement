using SharedKernel.Errors;

namespace Application.ProductReviews.Errors;

/// <summary>
/// Exception thrown when a user attempts to leave a review for a product they
/// haven't purchased.
/// </summary>
public class UserCannotLeaveReviewException : BaseException
{
    private const string DefaultTitle = "Review Not Allowed";

    private const string DefaultMessage =
        "You cannot leave a review for this" +
        " product because you have not purchased it";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotAllowed;

    internal UserCannotLeaveReviewException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserCannotLeaveReviewException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserCannotLeaveReviewException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
