using Microsoft.Extensions.Logging;

namespace Application.ProductFeedback.Queries.GetProductFeedback;

internal sealed partial class GetProductFeedbackQueryHandler
{
    private readonly ILogger<GetProductFeedbackQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating product feedback retrieval for product with id {ProductId}."
    )]
    private partial void LogInitiatingProductFeedbackRetrieval(string productId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Using specifications: {Specification1} and {Specification2}."
    )]
    private partial void LogProductFeedbackSpecifications(
        string specification1,
        string specification2
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Successfully retrieved {FeedbackCount} feedback items."
    )]
    private partial void LogProductFeedbackRetrievedSuccessfully(int feedbackCount);
}
