using FluentValidation;

namespace Application.Categories.Commands.UpdateCategory;

internal class UpdateCategoryCommandValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
