using System.Net;

namespace Application.Common.Errors;

/// <summary>
/// Represents HTTP exceptions.
/// Defaults for Internal Server Error.
/// </summary>
public class HttpException : Exception
{
    /// <summary>
    /// Default message for exceptions.
    /// </summary>
    private const string DefaultMessage = "An unexpected error occurred. Please try again later.";
    /// <summary>
    /// Default title for exceptions.
    /// </summary>
    private const string DefaultTitle = "Internal Server Error.";
    /// <summary>
    /// Gets the status code of the exception.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    /// <summary>
    /// Gets the title of the exception.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with default
    /// status code of <see cref="HttpStatusCode.InternalServerError"/> and default title.
    /// </summary>
    public HttpException() : base(DefaultMessage)
    {
        StatusCode = HttpStatusCode.InternalServerError;
        Title = DefaultTitle;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with default
    /// status code of <see cref="HttpStatusCode.InternalServerError"/> and default title.
    /// </summary>
    /// <param name="message">Details of the exception and how to proceed.</param>
    public HttpException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.InternalServerError;
        Title = DefaultTitle;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with default
    /// status code of <see cref="HttpStatusCode.InternalServerError"/> and default title.
    /// </summary>
    /// <param name="message">Details of the exception and how to proceed.</param>
    /// <param name="innerException">Inner exception.</param>
    public HttpException(string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = HttpStatusCode.InternalServerError;
        Title = DefaultTitle;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="message">Details of the exception and how to proceed.</param>
    /// <param name="title">Title of the exception.</param>
    /// <param name="statusCode">Status code of the exception.</param>
    public HttpException(string message, string title, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="message">Details of the exception and how to proceed.</param>
    /// <param name="title">Title of the exception.</param>
    /// <param name="statusCode">Status code of the exception.</param>
    /// <param name="innerException">Inner exception.</param>
    public HttpException(string message, string title, HttpStatusCode statusCode, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        Title = title;
    }
}
