using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.ShippingMethods.Errors;

using Domain.ShippingMethodAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Commands.DeleteShippingMethod;

internal sealed partial class DeleteShippingMethodCommandHandler
    : IRequestHandler<DeleteShippingMethodCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShippingMethodRepository _shippingMethodRepository;

    public DeleteShippingMethodCommandHandler(
        IUnitOfWork unitOfWork,
        IShippingMethodRepository shippingMethodRepository,
        ILogger<DeleteShippingMethodCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _shippingMethodRepository = shippingMethodRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteShippingMethodCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingShippingMethodDeletion(request.ShippingMethodId);

        var shippingMethodId = ShippingMethodId.Create(request.ShippingMethodId);

        var shippingMethodToDelete = await _shippingMethodRepository.FindByIdAsync(
            shippingMethodId,
            cancellationToken
        );

        if (shippingMethodToDelete == null)
        {
            LogShippingMethodNotFound();
            throw new ShippingMethodNotFoundException();
        }

        _shippingMethodRepository.RemoveOrDeactivate(shippingMethodToDelete);

        await _unitOfWork.SaveChangesAsync();
        LogShippingMethodDeletedSuccessfully();

        return Unit.Value;
    }
}
