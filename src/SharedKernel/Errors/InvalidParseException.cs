namespace SharedKernel.Errors;

/// <summary>
/// Exception thrown when it is not possible to parse a value.
/// </summary>
public class InvalidParseException : BaseException
{
    private const string DefaultTitle = "Invalid Parse Operation";
    private const string DefaultMessage =
        "There was an error when trying to parse a value";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidParseException"/> class.
    /// </summary>
    public InvalidParseException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidParseException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public InvalidParseException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidParseException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InvalidParseException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
