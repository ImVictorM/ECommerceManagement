using SharedKernel.ValueObjects;

using FluentValidation;

namespace Application.Common.Validation;

/// <summary>
/// Defines common validation extension methods.
/// </summary>
public static class RuleBuilderExtensions
{
    /// <summary>
    /// Checks if the given email address is valid by ensuring it is not empty
    /// and follows a valid email pattern.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">
    /// The current <see cref="IRuleBuilder{T, TProperty}"/> instance.
    /// </param>
    /// <returns>
    /// An <see cref="IRuleBuilder{T, TProperty}"/> configured with the specified
    /// validation rule.
    /// </returns>
    public static IRuleBuilder<T, string> IsValidEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .NotEmpty()
            .Must(Email.IsValidEmail)
            .WithMessage("'Email' does not follow the required pattern.");
    }

    /// <summary>
    /// Checks if the given name is valid by ensuring it is not empty and has a
    /// minimum length of three characters.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">
    /// The current <see cref="IRuleBuilder{T, TProperty}"/> instance.
    /// </param>
    /// <returns>
    /// An <see cref="IRuleBuilder{T, TProperty}"/> configured with the specified
    /// validation rule.
    /// </returns>
    public static IRuleBuilder<T, string> IsValidUserName<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("'Name' must be at least 3 characters long.");
    }
}
