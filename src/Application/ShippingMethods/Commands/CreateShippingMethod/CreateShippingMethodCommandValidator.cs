using FluentValidation;

namespace Application.ShippingMethods.Commands.CreateShippingMethod;

internal class CreateShippingMethodCommandValidator
    : AbstractValidator<CreateShippingMethodCommand>
{
    public CreateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.EstimatedDeliveryDays).GreaterThanOrEqualTo(1);
    }
}
