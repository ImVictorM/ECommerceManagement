namespace SharedKernel.Errors;

/// <summary>
/// Exception thrown when an argument is out of range.
/// </summary>
public class OutOfRangeException : BaseException
{
    private const string DefaultTitle = "Argument Out Of Range";
    private const string DefaultMessage =
        "The argument is out of the allowed range";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    /// <summary>
    /// Initiates a new instance of the <see cref="OutOfRangeException"/> class.
    /// </summary>
    public OutOfRangeException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="OutOfRangeException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public OutOfRangeException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="OutOfRangeException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public OutOfRangeException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
