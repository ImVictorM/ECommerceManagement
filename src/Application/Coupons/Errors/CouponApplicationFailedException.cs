using SharedKernel.Errors;

namespace Application.Coupons.Errors;

/// <summary>
/// Exception thrown when a coupon cannot be applied to an order 
/// due to order constraints or eligibility requirements.
/// </summary>
public class CouponApplicationFailedException : BaseException
{
    private const string DefaultTitle = "Coupon Application Failed";
    private const string DefaultMessage =
        "The coupon could not be applied because" +
        " the order does not meet the required conditions.";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal CouponApplicationFailedException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal CouponApplicationFailedException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal CouponApplicationFailedException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
