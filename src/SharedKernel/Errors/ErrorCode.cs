namespace SharedKernel.Errors;

/// <summary>
/// Represents a predefined set of error codes.
/// </summary>
public sealed class ErrorCode
{
    /// <summary>
    /// Represents a "Not Found" error.
    /// </summary>
    public static readonly ErrorCode NotFound = new("NOT_FOUND");

    /// <summary>
    /// Represents a "Conflict" error, typically used when a resource already
    /// exists or there's a clash in the data.
    /// </summary>
    public static readonly ErrorCode Conflict = new("CONFLICT");

    /// <summary>
    /// Represents an "Internal Error," used when an unexpected exception occurs
    /// within the application.
    /// </summary>
    public static readonly ErrorCode InternalError = new("INTERNAL_ERROR");

    /// <summary>
    /// Represents an "Invalid Operation" error, used when some operation fails.
    /// </summary>
    public static readonly ErrorCode InvalidOperation = new("INVALID_OPERATION");

    /// <summary>
    /// Represents an "Validation Error" error.
    /// </summary>
    public static readonly ErrorCode ValidationError = new("VALIDATION_ERROR");

    /// <summary>
    /// Represents an "Not Allowed" error.
    /// </summary>
    public static readonly ErrorCode NotAllowed = new("NOT_ALLOWED");

    /// <summary>
    /// Gets the unique code representing the specific error.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorCode"/> class
    /// with the specified error code.
    /// </summary>
    /// <param name="code">The unique string identifier for the error.</param>
    private ErrorCode(string code)
    {
        Code = code;
    }
}
