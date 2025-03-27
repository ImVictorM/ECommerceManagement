using SharedKernel.Errors;

namespace Domain.ProductReviewAggregate.Errors;

/// <summary>
/// Represents an exception thrown when the star rating is not between the allowed
/// range.
/// </summary>
public class InvalidStarRatingRangeException : BaseException
{
    private const string DefaultTitle = "Invalid Star Rating";
    private const string DefaultMessage =
        "The star rating must be between 0 and 5 inclusive";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    internal InvalidStarRatingRangeException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidStarRatingRangeException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidStarRatingRangeException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
