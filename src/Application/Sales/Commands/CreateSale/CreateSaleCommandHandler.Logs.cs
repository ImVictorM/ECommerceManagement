using Microsoft.Extensions.Logging;

namespace Application.Sales.Commands.CreateSale;

internal sealed partial class CreateSaleCommandHandler
{
    private readonly ILogger<CreateSaleCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating new sale creation."
    )]
    private partial void LogInitiatingSaleCreation();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The new sale is valid. The sale object was created without errors."
    )]
    private partial void LogSaleCreated();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The sale products are eligible for the new sale."
    )]
    private partial void LogSaleProductsIsEligible();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message =
        "The sale was created and saved. " +
        "The operation was completed successfully. " +
        "Generated identifier: '{CreatedId}'."
    )]
    private partial void LogSaleCreatedAndSavedSuccessfully(string createdId);
}
