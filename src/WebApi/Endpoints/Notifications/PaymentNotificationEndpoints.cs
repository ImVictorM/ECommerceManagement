using Contracts.Notifications;

using Application.Payments.Commands.UpdatePaymentStatus;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces.Authentication;

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

        paymentNotificationGroup.MapPost("/", HandlePaymentStatusChangedNotification);
    }

    private async Task<Results<NoContent, UnauthorizedHttpResult>> HandlePaymentStatusChangedNotification(
        [FromHeader(Name = "X-Provider-Signature")] string providerSignature,
        [FromBody] PaymentStatusChangedNotification notification,
        HttpRequest request,
        IHmacSignatureProvider hmacSignatureProvider,
        ISender sender,
        IMapper mapper
    )
    {
        using var requestBodyReader = new StreamReader(request.Body);

        var payload = await requestBodyReader.ReadToEndAsync();

        var computedSignature = hmacSignatureProvider.ComputeHmac(payload);

        if (!hmacSignatureProvider.Verify(computedSignature, providerSignature))
        {
            return TypedResults.Unauthorized();
        }

        var command = mapper.Map<UpdatePaymentStatusCommand>(notification);

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
