using FluentValidation;

namespace Application.Categories.Commands.UpdateCategory;

/// <summary>
/// Command validator for the <see cref="UpdateCategoryCommand"/> command.
/// </summary>
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateCategoryCommandValidator"/> class.
    /// </summary>
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
