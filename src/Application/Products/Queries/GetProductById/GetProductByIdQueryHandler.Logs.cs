using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductById;

internal sealed partial class GetProductByIdQueryHandler
{
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating retrieving product by id. Product id: {Id}.")]
    private partial void LogInitiateRetrievingProductById(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The product cannot be found. Either the product does not exist or it is inactive."
    )]
    private partial void LogProductDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The product was retrieved with its corresponding category names."
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
        Message = "The product was retrieved with its category names and calculate price on sale." +
        " Operation complete successfully."
    )]
    private partial void LogProductRetrievedSuccessfully();
}
