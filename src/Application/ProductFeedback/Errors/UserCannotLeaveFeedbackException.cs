using SharedKernel.Errors;

namespace Application.ProductFeedback.Errors;

/// <summary>
/// Exception thrown when a user attempts to leave feedback for a product they
/// haven't purchased.
/// </summary>
public class UserCannotLeaveFeedbackException : BaseException
{
    private const string DefaultTitle = "Feedback Not Allowed";

    private const string DefaultMessage =
        "You cannot leave feedback for this" +
        " product because you have not purchased it";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotAllowed;

    internal UserCannotLeaveFeedbackException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserCannotLeaveFeedbackException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserCannotLeaveFeedbackException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
