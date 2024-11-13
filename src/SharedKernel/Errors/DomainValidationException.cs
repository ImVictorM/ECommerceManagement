namespace SharedKernel.Errors;

/// <summary>
/// Exception related to domain entity validations.
/// </summary>
public sealed class DomainValidationException : BaseException
{
    private const string DefaultTitle = "Domain Validation Error";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    /// <summary>
    /// Initiates a new instance of the <see cref="DomainValidationException"/> class.
    /// </summary>
    public DomainValidationException() : base("An error occurred while validating an entity", DefaultTitle, _defaultErrorCode) { }

    /// <summary>
    /// Initiates a new instance of the <see cref="DomainValidationException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public DomainValidationException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="DomainValidationException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DomainValidationException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
