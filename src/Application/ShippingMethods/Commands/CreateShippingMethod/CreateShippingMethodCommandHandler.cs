using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.DTOs.Results;

using Domain.ShippingMethodAggregate;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Commands.CreateShippingMethod;

internal sealed partial class CreateShippingMethodCommandHandler
    : IRequestHandler<CreateShippingMethodCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShippingMethodRepository _shippingMethodRepository;

    public CreateShippingMethodCommandHandler(
        IUnitOfWork unitOfWork,
        IShippingMethodRepository shippingMethodRepository,
        ILogger<CreateShippingMethodCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _shippingMethodRepository = shippingMethodRepository;
        _logger = logger;
    }

    public async Task<CreatedResult> Handle(
        CreateShippingMethodCommand request,
        CancellationToken cancellationToken
    )
    {
        LogCreatingShippingMethod(request.Name);

        var shippingMethod = ShippingMethod.Create(
            request.Name,
            request.Price,
            request.EstimatedDeliveryDays
        );

        LogShippingMethodCreated();

        await _shippingMethodRepository.AddAsync(shippingMethod);

        await _unitOfWork.SaveChangesAsync();

        var createdId = shippingMethod.Id.ToString();

        LogShippingMethodSaved(createdId);

        return new CreatedResult(createdId);
    }
}
