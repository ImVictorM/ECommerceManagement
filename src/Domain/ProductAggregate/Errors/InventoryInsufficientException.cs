using SharedKernel.Errors;

namespace Domain.ProductAggregate.Errors;

/// <summary>
/// Represents an exception thrown when the product does not have current available stock
/// to complete an operation.
/// </summary>
public class InventoryInsufficientException : BaseException
{
    private const string DefaultTitle = "Inventory Insufficient";
    private const string DefaultMessage = "The product does not have available stock to complete the operation";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal InventoryInsufficientException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InventoryInsufficientException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InventoryInsufficientException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
