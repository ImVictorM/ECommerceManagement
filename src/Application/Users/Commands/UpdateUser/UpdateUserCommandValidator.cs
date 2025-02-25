using Application.Common.Validation;

using FluentValidation;

namespace Application.Users.Commands.UpdateUser;

internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name).IsValidUserName();

        RuleFor(x => x.Email).IsValidEmail();
    }
}
