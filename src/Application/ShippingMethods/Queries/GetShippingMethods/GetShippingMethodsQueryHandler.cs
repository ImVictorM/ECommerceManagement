using Application.Common.Persistence.Repositories;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.ShippingMethods.DTOs.Results;

namespace Application.ShippingMethods.Queries.GetShippingMethods;

internal sealed partial class GetShippingMethodsQueryHandler
    : IRequestHandler<GetShippingMethodsQuery, IReadOnlyList<ShippingMethodResult>>
{
    private readonly IShippingMethodRepository _shippingMethodRepository;

    public GetShippingMethodsQueryHandler(
        IShippingMethodRepository shippingMethodRepository,
        ILogger<GetShippingMethodsQueryHandler> logger
    )
    {
        _shippingMethodRepository = shippingMethodRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ShippingMethodResult>> Handle(
        GetShippingMethodsQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingShippingMethodsRetrieval();

        var shippingMethods = await _shippingMethodRepository.FindAllAsync(
            cancellationToken: cancellationToken
        );

        var result = shippingMethods
            .Select(ShippingMethodResult.FromShippingMethod)
            .ToList();

        LogShippingMethodsQuantityRetrieved(result.Count);

        return result;
    }
}
