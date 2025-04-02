using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProducts;

internal sealed partial class GetProductsQueryHandler
{
    private readonly ILogger<GetProductsQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating products retrieval."
    )]
    private partial void LogInitiatedRetrievingProducts();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Pagination details - Current page: {Page}, Page size: {PageSize}."
    )]
    private partial void LogPaginationDetails(int page, int pageSize);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "Categories filtering details - " +
        "Filter by categories: {CategoryIds}"
    )]
    private partial void LogCategoriesFilterDetails(string? categoryIds);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "{Quantity} products has been retrieved."
    )]
    private partial void LogProductsRetrieved(int quantity);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The discounted price has been calculated for each product."
   )]
    private partial void LogProductsDiscountedPriceCalculated();

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message =
        "The products has been retrieved with their respective discounted price." +
        " The operation was completed successfully."
    )]
    private partial void LogProductsRetrievedSuccessfully();
}
