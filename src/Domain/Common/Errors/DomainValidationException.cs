namespace Domain.Common.Errors;

/// <summary>
/// Exception related to domain entity validations.
/// </summary>
public sealed class DomainValidationException : Exception
{
    /// <summary>
    /// Initiates a new instance of the <see cref="DomainValidationException"/> class.
    /// </summary>
    public DomainValidationException() : base("An error occurred while validating an entity.") { }

    /// <summary>
    /// Initiates a new instance of the <see cref="DomainValidationException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public DomainValidationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="DomainValidationException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DomainValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
