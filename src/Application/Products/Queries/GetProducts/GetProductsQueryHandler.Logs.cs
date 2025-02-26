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
        Message = "Categories filtering details - Filter by categories: {Categories}"
    )]
    private partial void LogCategoriesFilterDetails(string? categories);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The products were retrieved. Quantity of products found: {Quantity}."
    )]
    private partial void LogProductsRetrieved(int quantity);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The product prices on sale were calculated successfully for each product."
   )]
    private partial void LogProductsPriceCalculated();

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "The products were retrieved with their respective price on sale and categories." +
        " Operation complete successfully."
    )]
    private partial void LogProductsRetrievedSuccessfully();
}
