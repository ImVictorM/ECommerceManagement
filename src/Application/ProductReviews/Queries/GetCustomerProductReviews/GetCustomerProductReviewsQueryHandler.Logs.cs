using Microsoft.Extensions.Logging;

namespace Application.ProductReviews.Queries.GetCustomerProductReviews;

internal sealed partial class GetCustomerProductReviewsQueryHandler
{
    private readonly ILogger<GetCustomerProductReviewsQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating reviews retrieval for customer with identifier '{CustomerId}'."
    )]
    private partial void LogInitiatingCustomerProductReviewsRetrieval(
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
        Message =
        "{ReviewsCount} has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogCustomerProductReviewsRetrievedSuccessfully(
        int reviewsCount
    );
}
