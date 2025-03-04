using FluentValidation;

namespace Application.ProductFeedback.Commands.LeaveProductFeedback;

internal class LeaveProductFeedbackCommandValidator : AbstractValidator<LeaveProductFeedbackCommand>
{
    public LeaveProductFeedbackCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(60);
    }
}
