namespace SharedKernel.Errors;

/// <summary>
/// The base exception for generic errors.
/// </summary>
public class BaseException : Exception
{
    /// <summary>
    /// Gets the exception title.
    /// </summary>
    public string Title { get; init; } = "Application Error";

    /// <summary>
    /// Gets the exception error code.
    /// </summary>
    public ErrorCode ErrorCode { get; init; } = ErrorCode.InternalError;

    /// <summary>
    /// Additional contextual information for the error.
    /// </summary>
    public Dictionary<string, object?> Context { get; } = [];


    /// <summary>
    /// Initiates a new instance of the <see cref="BaseException"/> with default properties.
    /// </summary>
    public BaseException() : base("There was an error while processing application logic")
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseException"/> with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public BaseException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseException"/> with custom title and message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="title">The exception title.</param>
    public BaseException(string message, string title) : base(message)
    {
        Title = title;
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseException"/> with custom title, message, and error code.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="title">The exception title.</param>
    /// <param name="errorCode">The exception error code.</param>
    public BaseException(string message, string title, ErrorCode errorCode) : base(message)
    {
        Title = title;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseException"/> with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public BaseException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseException"/> with custom message, title, and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="title">The exception title.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public BaseException(string message, string title, Exception innerException) : base(message, innerException)
    {
        Title = title;
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseException"/> with custom properties.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="title">The exception title.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    /// <param name="errorCode">The exception error code.</param>
    public BaseException(string message, string title, ErrorCode errorCode, Exception innerException) : base(message, innerException)
    {
        Title = title;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Adds additional contextual information to the exception.
    /// </summary>
    public BaseException WithContext(string key, object value)
    {
        Context[key] = value;
        return this;
    }
}
