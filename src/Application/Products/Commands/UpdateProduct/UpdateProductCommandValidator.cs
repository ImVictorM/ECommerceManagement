using FluentValidation;

namespace Application.Products.Commands.UpdateProduct;

/// <summary>
/// Validates the <see cref="UpdateProductCommand"/> command.
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductCommandValidator"/> class.
    /// </summary>
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.BasePrice).GreaterThan(0);
        RuleFor(x => x.CategoriesIds).NotEmpty();
        RuleFor(x => x.Images).NotEmpty();
    }
}
