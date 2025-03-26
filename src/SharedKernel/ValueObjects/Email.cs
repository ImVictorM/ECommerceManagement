using SharedKernel.Errors;
using SharedKernel.Models;

using System.Net.Mail;

namespace SharedKernel.ValueObjects;

/// <summary>
/// Represents an email.
/// </summary>
public sealed class Email : ValueObject
{
    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string Value { get; } = null!;

    private Email() { }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="value">The email address.</param>
    /// <returns>A new instance of the <see cref="Email"/> class.</returns>
    /// <exception cref="InvalidPatternException">
    /// An exception thrown when the email pattern is not valid.
    /// </exception>
    public static Email Create(string value)
    {
        if (!IsValidEmail(value))
        {
            throw new InvalidPatternException(
                $"The {value} does not correspond to a valid email"
            );
        }

        return new Email(value);
    }

    /// <summary>
    /// Validate if the given email address has a valid pattern.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>A boolean value indicating if the email is valid.</returns>
    public static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith('.'))
        {
            return false;
        }

        try
        {
            var mail = new MailAddress(email);

            return mail.Address == trimmedEmail;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value;
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
