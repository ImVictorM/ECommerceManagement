using Application.Common.Validation;

using FluentValidation;

namespace Application.Authentication.Commands.RegisterCustomer;

internal class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
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
