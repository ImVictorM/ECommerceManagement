using Application.Common.Persistence;
using Application.ShippingMethods.DTOs;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Queries.GetShippingMethods;

/// <summary>
/// Handles the <see cref="GetShippingMethodsQuery"/> query.
/// </summary>
public sealed partial class GetShippingMethodsQueryHandler : IRequestHandler<GetShippingMethodsQuery, IEnumerable<ShippingMethodResult>>
{
    private readonly IShippingMethodRepository _shippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodsQueryHandler"/> class.
    /// </summary>
    /// <param name="shippingMethodRepository">The shipping method repository.</param>
    /// <param name="logger">The logger.</param>
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
