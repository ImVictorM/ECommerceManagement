using SharedKernel.Errors;

namespace Application.Common.Errors;

/// <summary>
/// The exception that is thrown when a product being retrieved does not exist.
/// </summary>
public class ProductNotFoundException : BaseException
{
    private const string DefaultTitle = "Product Not Found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    /// <summary>
    /// Initiates a new default instance of the <see cref="ProductNotFoundException"/> class.
    /// </summary>
    public ProductNotFoundException() : base("The product being queried was not found", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductNotFoundException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public ProductNotFoundException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductNotFoundException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public ProductNotFoundException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
