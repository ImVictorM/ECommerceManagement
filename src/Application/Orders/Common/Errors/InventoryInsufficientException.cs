using SharedKernel.Errors;

namespace Application.Orders.Common.Errors;

/// <summary>
/// The exception that is thrown when the order cannot be complete because of insufficient inventory.
/// </summary>
public class InventoryInsufficientException : BaseException
{
    private const string DefaultTitle = "Inventory Insufficient";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    /// <summary>
    /// Initiates a new default instance of the <see cref="InventoryInsufficientException"/> class.
    /// </summary>
    public InventoryInsufficientException() :
        base(
            "The order cannot be complete because some of the products does not have sufficient inventory",
            DefaultTitle,
            _defaultErrorCode
        )
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InventoryInsufficientException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public InventoryInsufficientException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InventoryInsufficientException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public InventoryInsufficientException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
