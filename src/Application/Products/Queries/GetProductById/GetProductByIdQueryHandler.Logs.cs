using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductById;

internal sealed partial class GetProductByIdQueryHandler
{
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating retrieval of product with identifier '{Id}'."
    )]
    private partial void LogInitiatingProductRetrieval(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The product could not be found. " +
        "Either the product does not exist or is inactive."
    )]
    private partial void LogProductDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The product has been found and retrieved."
    )]
    private partial void LogProductRetrieved();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The product price on sale was calculated."
    )]
    private partial void LogProductPriceCalculated();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message =
        "The product has been retrieved, and its sale price has been calculated. " +
        "The operation was completed successfully."
    )]
    private partial void LogProductRetrievedSuccessfully();
}
