using Application.Payments.Commands.UpdatePaymentStatus;
using Application.Common.Security.Authentication;

using WebApi.Common.Utilities;

using Contracts.Payments;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.Payments;

/// <summary>
/// Endpoints related to payment webhooks.
/// </summary>
public class PaymentWebhookEndpoints : ICarterModule
{
    private const string BaseEndpoint = "webhooks/payments";

    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var paymentNotificationGroup = app
            .MapGroup(BaseEndpoint)
            .WithTags("PaymentWebhooks")
            .WithOpenApi();

        paymentNotificationGroup
            .MapPost("/", HandlePaymentStatusChanged)
            .WithName("HandlePaymentStatusChangedNotification")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Handle Payment Status Changed Notification",
                Description = "Updates internal data based on the changed payment status."
            });
    }

    private async Task<Results<NoContent, BadRequest, UnauthorizedHttpResult>> HandlePaymentStatusChanged(
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

        var notification = await JsonSerializerUtils.DeserializeFromWebAsync<PaymentStatusChangedRequest>(
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
