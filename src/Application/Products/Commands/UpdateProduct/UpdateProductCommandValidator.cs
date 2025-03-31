using FluentValidation;

namespace Application.Products.Commands.UpdateProduct;

internal class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.BasePrice).GreaterThan(0);
        RuleFor(x => x.CategoryIds).NotEmpty();
        RuleFor(x => x.Images).NotEmpty();
    }
}
