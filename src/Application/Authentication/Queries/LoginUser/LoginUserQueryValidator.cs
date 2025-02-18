using FluentValidation;

namespace Application.Authentication.Queries.LoginUser;

/// <summary>
/// Validates the <see cref="LoginUserQuery"/> input.
/// </summary>
public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginUserQueryValidator"/> class.
    /// </summary>
    public LoginUserQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
