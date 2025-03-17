using SharedKernel.Errors;

namespace Application.Categories.Errors;

/// <summary>
/// The exception that is thrown when a category being retrieved does not exist.
/// </summary>
public class CategoryNotFoundException : BaseException
{
    private const string DefaultTitle = "Category Not Found";
    private const string DefaultMessage = "The category being queried was not found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal CategoryNotFoundException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal CategoryNotFoundException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal CategoryNotFoundException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
