using Microsoft.Extensions.Logging;

namespace Application.ShippingMethods.Commands.CreateShippingMethod;

internal sealed partial class CreateShippingMethodCommandHandler
{
    private readonly ILogger<CreateShippingMethodCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating new shipping method creation. " +
        "Shipping method name: {ShippingMethodName}."
    )]
    private partial void LogCreatingShippingMethod(string shippingMethodName);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The shipping method has been created. " +
        "Initiating persistence."
    )]
    private partial void LogShippingMethodCreated();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The new shipping method has been created and saved with" +
        " the identifier '{Id}'. The operation was completed successfully."
    )]
    private partial void LogShippingMethodSaved(string id);
}
