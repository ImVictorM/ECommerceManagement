using Microsoft.Extensions.Logging;

namespace Application.ProductFeedback.Commands.LeaveProductFeedback;

internal sealed partial class LeaveProductFeedbackCommandHandler
{
    private readonly ILogger<LeaveProductFeedbackCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiate to leave product feedback for product with id {ProductId}."
    )]
    private partial void LogInitiateLeavingProductFeedback(string productId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Current user leaving feedback identifier: {UserId}."
    )]
    private partial void LogCurrentUserLeavingFeedback(string userId);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The current user cannot leave a feedback for the product " +
        "because they have not purchased the product."
    )]
    private partial void LogUserCannotLeaveFeedbackForProduct();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The current user is allowed to leave a feedback for the product."
    )]
    private partial void LogUserAllowedToLeaveFeedback();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The feedback was created without any errors."
    )]
    private partial void LogFeedbackCreated();

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "The feedback was created and saved." +
        " The operation complete successfully."
    )]
    private partial void LogFeedbackSavedSuccessfully();
}
