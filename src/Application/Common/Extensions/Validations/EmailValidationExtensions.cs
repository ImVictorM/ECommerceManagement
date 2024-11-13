using FluentValidation;
using SharedKernel.ValueObjects;

namespace Application.Common.Extensions.Validations;

/// <summary>
/// Defines common validation extension methods for email addresses.
/// </summary>
public static class EmailValidationExtensions
{
    /// <summary>
    /// Checks if the given email address is valid by ensuring it is not empty and follows a valid email pattern.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The <see cref="IRuleBuilder{T, TProperty}"/> instance that this extension method is called on.</param>
    /// <returns>An <see cref="IRuleBuilder{T, TProperty}"/> configured with the specified validation rules.</returns>
    public static IRuleBuilder<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Must(Email.IsValidEmail).WithMessage("'Email' does not follow the required pattern.");
    }
}
