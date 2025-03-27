using Microsoft.Extensions.Logging;

namespace Application.ProductReviews.Commands.LeaveProductReview;

internal sealed partial class LeaveProductReviewCommandHandler
{
    private readonly ILogger<LeaveProductReviewCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating to leave review for product with identifier '{ProductId}'."
    )]
    private partial void LogInitiateLeavingProductReview(string productId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Current user leaving review identifier: '{UserId}'."
    )]
    private partial void LogCurrentUserLeavingReview(string userId);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The current user cannot leave a review for the product " +
        "because they have not purchased the product."
    )]
    private partial void LogUserCannotLeaveReviewForProduct();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The current user is allowed to leave a review for the product."
    )]
    private partial void LogUserAllowedToLeaveReview();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The review was created without any errors."
    )]
    private partial void LogReviewCreated();

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message =
        "The review was created and saved." +
        " The operation was completed successfully."
    )]
    private partial void LogReviewSavedSuccessfully();
}
