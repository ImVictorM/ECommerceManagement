using Microsoft.Extensions.Logging;

namespace Application.ProductReviews.Queries.GetProductReviews;

internal sealed partial class GetProductReviewsQueryHandler
{
    private readonly ILogger<GetProductReviewsQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating reviews retrieval for product with identifier '{ProductId}'."
    )]
    private partial void LogInitiatingProductReviewsRetrieval(string productId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Using specifications: {Specification1} and {Specification2}."
    )]
    private partial void LogProductReviewsSpecifications(
        string specification1,
        string specification2
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "{ReviewsCount} has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogProductReviewsRetrievedSuccessfully(int reviewsCount);
}
