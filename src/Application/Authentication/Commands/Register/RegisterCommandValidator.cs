using Domain.Common.ValueObjects;
using FluentValidation;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Validates the <see cref="RegisterCommand"/> input.
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3).WithMessage("'Name' must be at least 3 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6).WithMessage("'Password' must be at least 6 characters long.")
            .Matches(@"[a-z]").WithMessage("'Password' must contain at least one character.")
            .Matches(@"[0-9]").WithMessage("'Password' must contain at least one digit.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .Must(Email.IsValidEmail).WithMessage("'Email' does not follow the required pattern.");
    }
}
