using Microsoft.Extensions.Logging;

namespace Application.Payments.Commands.UpdatePaymentStatus;

public sealed partial class UpdatePaymentStatusCommandHandler
{
    private readonly ILogger<UpdatePaymentStatusCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating payment status update. Payment identifier: {PaymentId}."
    )]
    private partial void LogInitiatingPaymentStatusUpdate(string paymentId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The payment status could not be updated because the payment does not exist."
    )]
    private partial void LogPaymentNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Updating payment status. Current payment status: {CurrentStatus}."
    )]
    private partial void LogUpdatingPaymentStatus(string currentStatus);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Payment status updated to {CurrentStatus}. The operation complete successfully."
    )]
    private partial void LogPaymentUpdatedSuccessfully(string currentStatus);
}
