using System.Net;

namespace Application.Common.Errors;

/// <summary>
/// Represents a bad request exception.
/// </summary>
public sealed class BadRequestException : HttpException
{
    /// <summary>
    /// Default bad request message in case it is needed.
    /// </summary>
    private const string DefaultMessage = "The request could not be understood by the server due to malformed syntax.";

    /// <summary>
    /// Bad request exception title.
    /// </summary>
    private const string BadRequestTitle = "Bad Request";

    /// <summary>
    /// Bad request exception status code.
    /// </summary>
    private const HttpStatusCode BadRequestStatusCode = HttpStatusCode.BadRequest;

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// Uses the default properties.
    /// </summary>
    public BadRequestException() : base(DefaultMessage, BadRequestTitle, BadRequestStatusCode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// Uses the default BadRequest status code and title.
    /// </summary>
    /// <param name="message">The error message.</param>
    public BadRequestException(string message)
        : base(message, BadRequestTitle, BadRequestStatusCode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// Uses the default BadRequest status code and title, and provides an inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public BadRequestException(string message, Exception innerException)
        : base(message, BadRequestTitle, BadRequestStatusCode, innerException)
    {
    }
}
