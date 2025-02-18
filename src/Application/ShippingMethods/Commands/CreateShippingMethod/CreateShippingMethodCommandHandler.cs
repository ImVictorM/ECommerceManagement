using Application.Common.Persistence;
using Application.Common.DTOs;

using Domain.ShippingMethodAggregate;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Commands.CreateShippingMethod;

/// <summary>
/// Handles the <see cref="CreateShippingMethodCommand"/> command.
/// </summary>
public sealed partial class CreateShippingMethodCommandHandler : IRequestHandler<CreateShippingMethodCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public CreateShippingMethodCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateShippingMethodCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(CreateShippingMethodCommand request, CancellationToken cancellationToken)
    {
        LogCreatingShippingMethod(request.Name);

        var shippingMethod = ShippingMethod.Create(
            request.Name,
            request.Price,
            request.EstimatedDeliveryDays
        );

        LogShippingMethodCreated();

        await _unitOfWork.ShippingMethodRepository.AddAsync(shippingMethod);

        await _unitOfWork.SaveChangesAsync();

        LogShippingMethodSaved(shippingMethod.Id.ToString());

        return new CreatedResult(shippingMethod.Id.ToString());
    }
}
