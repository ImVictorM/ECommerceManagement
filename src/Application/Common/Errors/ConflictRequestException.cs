using System.Net;

namespace Application.Common.Errors;

/// <summary>
/// Represents a Conflict exception.
/// </summary>
public sealed class ConflictRequestException : HttpException
{
    /// <summary>
    /// Default Conflict message in case it is needed.
    /// </summary>
    private const string DefaultMessage = "The request could not be completed due to a conflict with the current state of the resource.";

    /// <summary>
    /// Conflict exception title.
    /// </summary>
    private const string ConflictTitle = "Conflict";

    /// <summary>
    /// Conflict exception status code.
    /// </summary>
    private const HttpStatusCode ConflictRequestStatusCode = HttpStatusCode.Conflict;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictRequestException"/> class.
    /// Uses the default properties.
    /// </summary>
    public ConflictRequestException() : base(DefaultMessage, ConflictTitle, ConflictRequestStatusCode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictRequestException"/> class.
    /// Uses the default Conflict status code and title.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ConflictRequestException(string message)
        : base(message, ConflictTitle, ConflictRequestStatusCode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictRequestException"/> class.
    /// Uses the default Conflict status code.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="title">The error title.</param>
    public ConflictRequestException(string message, string title)
        : base(message, title, ConflictRequestStatusCode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictRequestException"/> class.
    /// Uses the default BadRequest status code and title, and provides an inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ConflictRequestException(string message, Exception innerException)
        : base(message, ConflictTitle, ConflictRequestStatusCode, innerException)
    {
    }
}
