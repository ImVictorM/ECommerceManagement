using SharedKernel.Errors;

namespace Application.Coupons.Errors;

/// <summary>
/// Exception thrown when a coupon does not exist.
/// </summary>
public class CouponNotFoundException : BaseException
{
    private const string DefaultTitle = "Coupon Not Found";
    private const string DefaultMessage =
        "The specified coupon does not exist or has expired.";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal CouponNotFoundException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal CouponNotFoundException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal CouponNotFoundException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
