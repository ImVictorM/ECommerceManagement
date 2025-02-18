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
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetShippingMethodByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetShippingMethodByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ShippingMethodResult> Handle(GetShippingMethodByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingGetShippingMethodByIdQuery(request.ShippingMethodId);

        var shippingMethodId = ShippingMethodId.Create(request.ShippingMethodId);

        var shippingMethod = await _unitOfWork.ShippingMethodRepository.FindByIdAsync(shippingMethodId);

        if (shippingMethod == null)
        {
            LogShippingMethodNotFound();
            throw new ShippingMethodNotFoundException();
        }

        LogShippingMethodRetrievedSuccessfully();

        return new ShippingMethodResult(shippingMethod);
    }
}
