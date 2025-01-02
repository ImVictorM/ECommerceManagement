using FluentValidation;

namespace Application.Categories.Commands.CreateCategory;

/// <summary>
/// Validator for the <see cref="CreateCategoryCommand"/> command.
/// </summary>
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCategoryCommandValidator"/> class.
    /// </summary>
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
