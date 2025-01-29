using SharedKernel.Errors;

namespace Application.Orders.Errors;

/// <summary>
/// Exception thrown when an invalid coupon is applied to an order.
/// </summary>
public class InvalidCouponAppliedException : BaseException
{
    private const string DefaultTitle = "Invalid Coupon Applied";
    private const string DefaultMessage = "The coupon applied to the order is invalid";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    internal InvalidCouponAppliedException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidCouponAppliedException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidCouponAppliedException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
