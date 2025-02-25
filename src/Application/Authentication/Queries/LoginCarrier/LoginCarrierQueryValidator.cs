using FluentValidation;

namespace Application.Authentication.Queries.LoginCarrier;

internal class LoginCarrierQueryValidator : AbstractValidator<LoginCarrierQuery>
{
    public LoginCarrierQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
