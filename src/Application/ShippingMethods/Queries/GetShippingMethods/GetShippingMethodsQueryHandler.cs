using Application.Common.Persistence.Repositories;
using Application.ShippingMethods.DTOs;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Queries.GetShippingMethods;

internal sealed partial class GetShippingMethodsQueryHandler
    : IRequestHandler<GetShippingMethodsQuery, IEnumerable<ShippingMethodResult>>
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

    /// <inheritdoc/>
    public async Task<IEnumerable<ShippingMethodResult>> Handle(
        GetShippingMethodsQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingShippingMethodsRetrieval();

        var shippingMethods = await _shippingMethodRepository.FindAllAsync(cancellationToken: cancellationToken);

        var result = shippingMethods.Select(s => new ShippingMethodResult(s)).ToList();

        LogShippingMethodsQuantityRetrieved(result.Count);

        return result;
    }
}
