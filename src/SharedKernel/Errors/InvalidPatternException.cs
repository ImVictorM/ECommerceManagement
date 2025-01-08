namespace SharedKernel.Errors;

/// <summary>
/// Exception thrown when a pattern is invalid/incorrect.
/// </summary>
public class InvalidPatternException : BaseException
{
    private const string DefaultTitle = "Invalid Pattern";
    private const string DefaultMessage = "The argument does not correspond to a valid pattern";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidPatternException"/> class.
    /// </summary>
    public InvalidPatternException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode) { }

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidPatternException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public InvalidPatternException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="InvalidPatternException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InvalidPatternException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
