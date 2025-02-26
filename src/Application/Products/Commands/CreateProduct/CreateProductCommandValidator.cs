using FluentValidation;

namespace Application.Products.Commands.CreateProduct;

internal class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.BasePrice).GreaterThan(0);
        RuleFor(x => x.InitialQuantity).GreaterThan(0);
        RuleFor(x => x.CategoryIds).NotEmpty();
        RuleFor(x => x.Images).NotEmpty();
    }
}
