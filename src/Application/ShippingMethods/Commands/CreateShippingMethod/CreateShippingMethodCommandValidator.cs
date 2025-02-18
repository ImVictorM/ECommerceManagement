using FluentValidation;

namespace Application.ShippingMethods.Commands.CreateShippingMethod;

/// <summary>
/// Validates the <see cref="CreateShippingMethodCommand"/> command inputs.
/// </summary>
public class CreateShippingMethodCommandValidator : AbstractValidator<CreateShippingMethodCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodCommandValidator"/> class.
    /// </summary>
    public CreateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.EstimatedDeliveryDays).GreaterThanOrEqualTo(1);
    }
}
