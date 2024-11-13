using FluentValidation;

namespace Application.Products.Commands.CreateProduct;

/// <summary>
/// Validates the fields for the <see cref="CreateProductCommand"/> command.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductCommandValidator"/> class.
    /// </summary>
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.InitialPrice).GreaterThan(0);
        RuleFor(x => x.InitialQuantity).GreaterThan(0);
        RuleFor(x => x.Categories).NotEmpty();
        RuleFor(x => x.Images).NotEmpty();
    }
}
