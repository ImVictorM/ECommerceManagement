using Microsoft.Extensions.Logging;

namespace Application.ProductFeedback.Queries.GetCustomerProductFeedback;

internal sealed partial class GetCustomerProductFeedbackQueryHandler
{
    private readonly ILogger<GetCustomerProductFeedbackQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating customer product feedback retrieval for customer " +
        "with id {CustomerId}."
    )]
    private partial void LogInitiatingCustomerProductFeedbackRetrieval(
        string customerId
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Using specifications: {Specification1} and {Specification2}."
    )]
    private partial void LogSpecifications(
        string specification1,
        string specification2
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Successfully retrieved {FeedbackCount} feedback items."
    )]
    private partial void LogCustomerProductFeedbackRetrievedSuccessfully(
        int feedbackCount
    );
}
