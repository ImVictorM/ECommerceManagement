using SharedKernel.Errors;

namespace Application.Coupons.Errors;

/// <summary>
/// Exception thrown when an unsupported coupon restriction type is encountered.
/// </summary>
public class NotSupportedRestrictionTypeException : BaseException
{
    private const string DefaultTitle = "Unsupported Restriction Type";
    private const string DefaultMessage =
        "The provided coupon restriction type is not supported";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    internal NotSupportedRestrictionTypeException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal NotSupportedRestrictionTypeException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal NotSupportedRestrictionTypeException(
        string message,
        Exception innerException
    )
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
