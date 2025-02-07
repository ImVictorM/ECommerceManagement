using FluentValidation;

namespace Application.Authentication.Queries.LoginCarrier;

/// <summary>
/// Validates the <see cref="LoginCarrierQuery"/> input.
/// </summary>
public class LoginCarrierQueryValidator : AbstractValidator<LoginCarrierQuery>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierQueryValidator"/> class.
    /// </summary>
    public LoginCarrierQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
