using Application.Common.Persistence.Repositories;
using Application.ShippingMethods.Errors;

using Domain.ShippingMethodAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.ShippingMethods.DTOs.Results;

namespace Application.ShippingMethods.Queries.GetShippingMethodById;

internal sealed partial class GetShippingMethodByIdQueryHandler
    : IRequestHandler<GetShippingMethodByIdQuery, ShippingMethodResult>
{
    private readonly IShippingMethodRepository _shippingMethodRepository;

    public GetShippingMethodByIdQueryHandler(
        IShippingMethodRepository shippingMethodRepository,
        ILogger<GetShippingMethodByIdQueryHandler> logger
    )
    {
        _shippingMethodRepository = shippingMethodRepository;
        _logger = logger;
    }

    public async Task<ShippingMethodResult> Handle(
        GetShippingMethodByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingShippingMethodRetrieval(request.ShippingMethodId);

        var shippingMethodId = ShippingMethodId.Create(request.ShippingMethodId);

        var shippingMethod = await _shippingMethodRepository.FindByIdAsync(
            shippingMethodId,
            cancellationToken
        );

        if (shippingMethod == null)
        {
            LogShippingMethodNotFound();
            throw new ShippingMethodNotFoundException();
        }

        LogShippingMethodRetrievedSuccessfully();

        return ShippingMethodResult.FromShippingMethod(shippingMethod);
    }
}
