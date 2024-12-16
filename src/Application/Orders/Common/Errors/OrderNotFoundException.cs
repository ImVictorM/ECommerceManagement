using SharedKernel.Errors;

namespace Application.Orders.Common.Errors;

/// <summary>
/// The exception that is thrown when the order being retrieved does not exist.
/// </summary>
public class OrderNotFoundException : BaseException
{
    private const string DefaultTitle = "Order Not Found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    /// <summary>
    /// Initiates a new default instance of the <see cref="OrderNotFoundException"/> class.
    /// </summary>
    public OrderNotFoundException() : base("The order being queried was not found", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderNotFoundException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public OrderNotFoundException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderNotFoundException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public OrderNotFoundException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
