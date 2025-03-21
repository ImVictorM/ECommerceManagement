using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Sales.Errors;

using Domain.SaleAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Sales.Commands.DeleteSale;

internal sealed partial class DeleteSaleCommandHandler
    : IRequestHandler<DeleteSaleCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSaleCommandHandler(
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteSaleCommandHandler> logger
    )
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteSaleCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingSaleDeletion(request.SaleId);

        var saleId = SaleId.Create(request.SaleId);

        var sale = await _saleRepository.FindByIdAsync(saleId, cancellationToken);

        if (sale == null)
        {
            LogSaleNotFound();
            throw new SaleNotFoundException();
        }

        _saleRepository.RemoveOrDeactivate(sale);

        await _unitOfWork.SaveChangesAsync();

        LogSaleDeletedSuccessfully(request.SaleId);

        return Unit.Value;
    }
}
