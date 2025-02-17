using Microsoft.Extensions.Logging;

namespace Application.ShippingMethods.Commands.CreateShippingMethod;

public sealed partial class CreateShippingMethodCommandHandler
{
    private readonly ILogger<CreateShippingMethodCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating new shipping method creation. Shipping method name: {ShippingMethodName}."
    )]
    private partial void LogCreatingShippingMethod(string shippingMethodName);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Shipping method created successfully. Initiating persistence."
    )]
    private partial void LogShippingMethodCreated();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Shipping method saved successfully. Generated identifier: {Id}."
    )]
    private partial void LogShippingMethodSaved(string id);
}
