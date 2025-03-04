using Microsoft.Extensions.Logging;

namespace Application.ProductFeedback.Commands.DeactivateCustomerProductFeedback;

internal sealed partial class DeactivateCustomerProductFeedbackCommandHandler
{
    private readonly ILogger<DeactivateCustomerProductFeedbackCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating customer product feedback deactivation for feedback" +
        " item with id {FeedbackId}"
    )]
    private partial void LogInitiatingCustomerProductFeedbackDeactivation(
        string feedbackId
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The operation could not complete because the requested" +
        " feedback item was not found."
    )]
    private partial void LogFeedbackNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Feedback deactivated. Operation complete successfully."
    )]
    private partial void LogCustomerProductFeedbackDeactivateSuccessfully();
}
