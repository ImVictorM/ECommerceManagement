using SharedKernel.Errors;

namespace Application.Orders.Commands.Common.Errors;

/// <summary>
/// The exception that is thrown when a product inventory cannot afford the current operation.
/// </summary>
public class InventoryUnavailableException : BaseException
{
    private const string DefaultTitle = "Inventory Unavailable";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    /// <summary>
    /// Initiates a new default instance of the <see cref="InventoryUnavailableException"/> class.
    /// </summary>
    public InventoryUnavailableException() : base("The inventory cannot deduct the current amount", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InventoryUnavailableException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public InventoryUnavailableException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InventoryUnavailableException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public InventoryUnavailableException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
