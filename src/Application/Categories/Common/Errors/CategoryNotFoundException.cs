using SharedKernel.Errors;

namespace Application.Categories.Common.Errors;

/// <summary>
/// The exception that is thrown when a category being retrieved does not exist.
/// </summary>
public class CategoryNotFoundException : BaseException
{
    private const string DefaultTitle = "Category Not Found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    /// <summary>
    /// Initiates a new default instance of the <see cref="CategoryNotFoundException"/> class.
    /// </summary>
    public CategoryNotFoundException() : base("The category being queried was not found", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="CategoryNotFoundException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public CategoryNotFoundException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="CategoryNotFoundException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public CategoryNotFoundException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
