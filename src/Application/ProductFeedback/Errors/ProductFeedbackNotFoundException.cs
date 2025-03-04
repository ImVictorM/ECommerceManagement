using Domain.ProductFeedbackAggregate.ValueObjects;

using SharedKernel.Errors;

namespace Application.ProductFeedback.Errors;

/// <summary>
/// Exception thrown when the requested product feedback is not found.
/// </summary>
public class ProductFeedbackNotFoundException : BaseException
{
    private const string DefaultTitle = "Feedback Not Found";

    private const string DefaultMessage =
        "The requested product feedback was not found.";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal ProductFeedbackNotFoundException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ProductFeedbackNotFoundException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ProductFeedbackNotFoundException(
        string message,
        Exception innerException
    )
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }

    internal ProductFeedbackNotFoundException(ProductFeedbackId feedbackId)
        : base(
            $"The product feedback with id '{feedbackId}' was not found.",
            DefaultTitle,
            _defaultErrorCode
        )
    {
    }
}
