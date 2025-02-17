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
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodsQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetShippingMethodsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetShippingMethodsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ShippingMethodResult>> Handle(GetShippingMethodsQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingShippingMethodsRetrieval();

        var shippingMethods = await _unitOfWork.ShippingMethodRepository.FindAllAsync();

        var result = shippingMethods.Select(s => new ShippingMethodResult(s)).ToList();

        LogShippingMethodsQuantityRetrieved(result.Count);

        return result;
    }
}
