using FluentValidation;

namespace Application.Coupons.Commands.CreateCoupon;

internal class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.UsageLimit).GreaterThanOrEqualTo(1);
        RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0);
    }
}
