using Application.Common.Extensions.Validations;
using FluentValidation;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Validates the <see cref="UpdateUserCommand"/> inputs.
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserCommandValidator"/> class.
    /// </summary>
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name).IsValidUserName();

        RuleFor(x => x.Email).IsValidEmail();
    }
}
