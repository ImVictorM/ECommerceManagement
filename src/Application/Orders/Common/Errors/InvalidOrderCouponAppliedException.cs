using SharedKernel.Errors;

namespace Application.Orders.Common.Errors;

/// <summary>
/// The exception that is thrown when the order has invalid coupons.
/// </summary>
public class InvalidOrderCouponAppliedException : BaseException
{
    private const string DefaultTitle = "Invalid Order Coupon Applied";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    /// <summary>
    /// Initiates a new default instance of the <see cref="InvalidOrderCouponAppliedException"/> class.
    /// </summary>
    public InvalidOrderCouponAppliedException() : base("The coupon cannot be applied to the given order", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidOrderCouponAppliedException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public InvalidOrderCouponAppliedException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidOrderCouponAppliedException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public InvalidOrderCouponAppliedException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
