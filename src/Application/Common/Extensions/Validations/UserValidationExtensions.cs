using FluentValidation;

namespace Application.Common.Extensions.Validations;

/// <summary>
/// Defines common validation extension methods for user persistence requests.
/// </summary>
public static class UserValidationExtensions
{
    /// <summary>
    /// Checks if the given name is valid by ensuring it is not empty and has a minimum length.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The <see cref="IRuleBuilder{T, TProperty}"/> instance that this extension method is called on.</param>
    /// <returns>An <see cref="IRuleBuilder{T, TProperty}"/> configured with the specified validation rules.</returns>
    public static IRuleBuilder<T, string> IsValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MinimumLength(3).WithMessage("'Name' must be at least 3 characters long.");
    }
}
