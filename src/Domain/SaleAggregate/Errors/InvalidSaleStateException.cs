using SharedKernel.Errors;

namespace Domain.SaleAggregate.Errors;

/// <summary>
/// Exception thrown when the sale is not in a valid state.
/// </summary>
public class InvalidSaleStateException : BaseException
{
    private const string DefaultTitle = "Invalid Sale State";
    private const string DefaultMessage = "The sale state is not valid";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal InvalidSaleStateException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidSaleStateException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidSaleStateException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
