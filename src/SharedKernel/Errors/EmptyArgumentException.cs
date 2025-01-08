namespace SharedKernel.Errors;

/// <summary>
/// Exception thrown when some argument is not expected to be empty.
/// </summary>
public class EmptyArgumentException : BaseException
{
    private const string DefaultTitle = "Empty Argument Not Expected";
    private const string DefaultMessage = "The argument should not be empty";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    /// <summary>
    /// Initiates a new instance of the <see cref="EmptyArgumentException"/> class.
    /// </summary>
    public EmptyArgumentException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode) { }

    /// <summary>
    /// Initiates a new instance of the <see cref="EmptyArgumentException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public EmptyArgumentException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="EmptyArgumentException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public EmptyArgumentException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
