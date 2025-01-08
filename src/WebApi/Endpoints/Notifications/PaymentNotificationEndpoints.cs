using Contracts.Notifications;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Application.Payments.Commands.UpdatePaymentStatus;

namespace WebApi.Endpoints.Notifications;

/// <summary>
/// Endpoints related to payment notifications.
/// </summary>
public class PaymentNotificationEndpoints : ICarterModule
{
    /// <summary>
    /// The base endpoint for the payment notifications.
    /// </summary>
    public const string BaseEndpoint = "notifications/payments";

    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var paymentNotificationGroup = app
            .MapGroup(BaseEndpoint)
            .WithTags("Notifications", "Payments")
            .WithOpenApi();

        paymentNotificationGroup.MapPost("/", ReceivePaymentStatusNotification);
    }

    private async Task<NoContent> ReceivePaymentStatusNotification(
        [FromBody] PaymentStatusNotification notification,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdatePaymentStatusCommand>(notification);

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
