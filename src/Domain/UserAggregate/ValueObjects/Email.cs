using System.Net.Mail;
using Domain.Common.Errors;
using Domain.Common.Models;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents an email.
/// </summary>
public sealed class Email : ValueObject
{
    /// <summary>
    /// Gets the email.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="value"></param>
    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="value">The email address.</param>
    /// <returns>A new instance of the <see cref="Email"/> class.</returns>
    /// <exception cref="DomainValidationException">An exception thrown when the email is not valid.</exception>
    public static Email Create(string value)
    {
        if (!IsValidEmail(value)) throw new DomainValidationException($"The {value} does not correspond to a valid email");

        return new Email(value);
    }

    /// <summary>
    /// Validate if the given email has a valid format.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>A boolean value indicating if the email is valid.</returns>
    public static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();
        if (trimmedEmail.EndsWith('.')) return false;

        try
        {
            var addr = new MailAddress(email);

            return addr.Address == trimmedEmail;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
