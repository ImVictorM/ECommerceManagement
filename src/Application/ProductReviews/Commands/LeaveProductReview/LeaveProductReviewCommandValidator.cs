using FluentValidation;

namespace Application.ProductReviews.Commands.LeaveProductReview;

internal class LeaveProductReviewCommandValidator
    : AbstractValidator<LeaveProductReviewCommand>
{
    public LeaveProductReviewCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(60);
    }
}
