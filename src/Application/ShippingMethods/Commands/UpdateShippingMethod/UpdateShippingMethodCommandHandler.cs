using Application.Common.Persistence;
using Application.ShippingMethods.Errors;

using Domain.ShippingMethodAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Commands.UpdateShippingMethod;

/// <summary>
/// Handles the <see cref="UpdateShippingMethodCommand"/> command.
/// </summary>
public sealed partial class UpdateShippingMethodCommandHandler : IRequestHandler<UpdateShippingMethodCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateShippingMethodCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateShippingMethodCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateShippingMethodCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateShippingMethodCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingShippingMethodUpdate(request.ShippingMethodId);

        var shippingMethodId = ShippingMethodId.Create(request.ShippingMethodId);

        var shippingMethod = await _unitOfWork.ShippingMethodRepository.FindByIdAsync(shippingMethodId);

        if (shippingMethod == null)
        {
            LogShippingMethodNotFound();
            throw new ShippingMethodNotFoundException();
        }

        shippingMethod.Update(
            name: request.Name,
            price: request.Price,
            estimatedDeliveryDays: request.EstimatedDeliveryDays
        );
        LogShippingMethodUpdated();

        await _unitOfWork.SaveChangesAsync();
        LogShippingMethodChangesSaved();

        return Unit.Value;
    }
}
