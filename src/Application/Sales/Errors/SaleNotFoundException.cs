using Domain.SaleAggregate.ValueObjects;

using SharedKernel.Errors;

namespace Application.Sales.Errors;

/// <summary>
/// Exception thrown when a sale being queried does not exist.
/// </summary>
public class SaleNotFoundException : BaseException
{
    private const string DefaultTitle = "Sale Not Found";
    private const string DefaultMessage =
        "The sale being queried was not found";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal SaleNotFoundException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal SaleNotFoundException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal SaleNotFoundException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }

    internal SaleNotFoundException(SaleId saleId)
        : base(
            $"The sale with id '{saleId}' was not found",
            DefaultTitle,
            _defaultErrorCode
        )
    {
    }
}
