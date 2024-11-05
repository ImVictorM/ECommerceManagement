using SharedKernel.Errors;

namespace Application.Users.Common.Errors;

/// <summary>
/// The exception that is thrown when a user being retrieved does not exist.
/// </summary>
public sealed class UserNotFoundException : BaseException
{
    private const string DefaultTitle = "User not found";

    /// <summary>
    /// Initiates a new default instance of the <see cref="UserNotFoundException"/> class.
    /// </summary>
    public UserNotFoundException() : base("The user being queried was not found", DefaultTitle)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserNotFoundException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UserNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserNotFoundException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
