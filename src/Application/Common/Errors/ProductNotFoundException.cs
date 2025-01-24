using SharedKernel.Errors;

namespace Application.Common.Errors;

/// <summary>
/// The exception thrown when a product being queried does not exist.
/// </summary>
public class ProductNotFoundException : BaseException
{
    private const string DefaultTitle = "Product Not Found";
    private const string DefaultMessage = "The product being queried was not found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal ProductNotFoundException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ProductNotFoundException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ProductNotFoundException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
