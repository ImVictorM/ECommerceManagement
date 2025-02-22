using Application.Common.Persistence;
using Application.ShippingMethods.DTOs;
using Application.ShippingMethods.Errors;

using Domain.ShippingMethodAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Queries.GetShippingMethodById;

/// <summary>
/// Handles the <see cref="GetShippingMethodByIdQueryHandler"/> query.
/// </summary>
public sealed partial class GetShippingMethodByIdQueryHandler : IRequestHandler<GetShippingMethodByIdQuery, ShippingMethodResult>
{
    private readonly IShippingMethodRepository _shippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="shippingMethodRepository">The shipping method repository.</param>
    /// <param name="logger">The logger.</param>
    public GetShippingMethodByIdQueryHandler(
        IShippingMethodRepository shippingMethodRepository,
        ILogger<GetShippingMethodByIdQueryHandler> logger
    )
    {
        _shippingMethodRepository = shippingMethodRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ShippingMethodResult> Handle(GetShippingMethodByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingGetShippingMethodByIdQuery(request.ShippingMethodId);

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

        return new ShippingMethodResult(shippingMethod);
    }
}
