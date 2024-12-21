using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductById;

public partial class GetProductByIdQueryHandler
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
        Message = "Product found."
    )]
    private partial void LogProductFound();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Calculating product price."
    )]
    private partial void LogCalculatingProductCurrentPrice();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "Getting product category names."
    )]
    private partial void LogGettingProductCategories();
}
