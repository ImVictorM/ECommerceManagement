using Contracts.Notifications;

using Application.Payments.Commands.UpdatePaymentStatus;
using Application.Common.Security.Authentication;

using WebApi.Common.Utils;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
            .WithTags("PaymentNotifications")
            .WithOpenApi();

        paymentNotificationGroup.MapPost("/", HandlePaymentStatusChangedNotification);
    }

    private async Task<Results<NoContent, BadRequest, UnauthorizedHttpResult>> HandlePaymentStatusChangedNotification(
        [FromHeader(Name = "X-Provider-Signature")] string providerSignature,
        HttpRequest request,
        IHmacSignatureProvider hmacSignatureProvider,
        ISender sender,
        IMapper mapper
    )
    {
        request.EnableBuffering();

        using var requestBodyReader = new StreamReader(request.Body);
        var payload = await requestBodyReader.ReadToEndAsync();

        var computedSignature = hmacSignatureProvider.ComputeHmac(payload);

        if (!hmacSignatureProvider.Verify(computedSignature, providerSignature))
        {
            return TypedResults.Unauthorized();
        }

        //  Rewind the stream to the beginning for deserialization
        request.Body.Position = 0;

        var notification = await JsonSerializerUtils.DeserializeFromWebAsync<PaymentStatusChangedNotification>(
            requestBodyReader.BaseStream
        );

        if (notification == null)
        {
            return TypedResults.BadRequest();
        }

        var command = mapper.Map<UpdatePaymentStatusCommand>(notification);

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
