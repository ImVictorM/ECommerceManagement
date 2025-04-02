using Microsoft.Extensions.Logging;

namespace Application.ProductReviews.Commands.DeactivateCustomerProductReview;

internal sealed partial class DeactivateCustomerProductReviewCommandHandler
{
    private readonly ILogger<DeactivateCustomerProductReviewCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating customer product review deactivation for" +
        " review with identifier '{ReviewId}'"
    )]
    private partial void LogInitiatingCustomerProductReviewDeactivation(
        string reviewId
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The operation could not be completed because the requested" +
        " review was not found."
    )]
    private partial void LogReviewNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The review has been deactivated. " +
        "The operation was completed successfully."
    )]
    private partial void LogCustomerProductReviewDeactivateSuccessfully();
}
