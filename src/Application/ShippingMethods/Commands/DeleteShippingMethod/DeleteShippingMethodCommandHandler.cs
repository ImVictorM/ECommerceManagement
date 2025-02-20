using Application.Common.Persistence;
using Application.ShippingMethods.Errors;

using Domain.ShippingMethodAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ShippingMethods.Commands.DeleteShippingMethod;

/// <summary>
/// Handles the <see cref="DeleteShippingMethodCommand"/> command.
/// </summary>
public sealed partial class DeleteShippingMethodCommandHandler : IRequestHandler<DeleteShippingMethodCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShippingMethodRepository _shippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteShippingMethodCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="shippingMethodRepository">The shipping method repository.</param>
    /// <param name="logger">The logger.</param>
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

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeleteShippingMethodCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingDeleteShippingMethod(request.ShippingMethodId);

        var shippingMethodToDeleteId = ShippingMethodId.Create(request.ShippingMethodId);

        var shippingMethodToDelete = await _shippingMethodRepository.FindByIdAsync(shippingMethodToDeleteId, cancellationToken);

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
