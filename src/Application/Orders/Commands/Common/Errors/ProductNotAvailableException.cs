using SharedKernel.Errors;

namespace Application.Orders.Commands.Common.Errors;

/// <summary>
/// The exception that is thrown when a product inventory cannot afford the current operation.
/// </summary>
public class ProductNotAvailableException : BaseException
{
    private const string DefaultTitle = "Product Not Available";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    /// <summary>
    /// Initiates a new default instance of the <see cref="ProductNotAvailableException"/> class.
    /// </summary>
    public ProductNotAvailableException() : base("The current product inventory is not available for this order", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductNotAvailableException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public ProductNotAvailableException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductNotAvailableException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public ProductNotAvailableException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
