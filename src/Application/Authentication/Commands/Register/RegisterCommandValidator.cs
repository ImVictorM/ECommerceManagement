using Application.Common.Validation;

using FluentValidation;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Validates the <see cref="RegisterCommand"/> inputs.
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name).IsValidUserName();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6).WithMessage("'Password' must be at least 6 characters long.")
            .Matches(@"[a-z]").WithMessage("'Password' must contain at least one character.")
            .Matches(@"[0-9]").WithMessage("'Password' must contain at least one digit.");

        RuleFor(x => x.Email).IsValidEmail();
    }
}
