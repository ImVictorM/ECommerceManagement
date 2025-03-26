using Application.Common.Persistence.Repositories;
using Application.Sales.Errors;
using Application.Sales.DTOs.Results;

using Domain.SaleAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Sales.Queries.GetSaleById;

internal sealed partial class GetSaleByIdQueryHandler
    : IRequestHandler<GetSaleByIdQuery, SaleResult>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleByIdQueryHandler(
        ISaleRepository saleRepository,
        ILogger<GetSaleByIdQueryHandler> logger
    )
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task<SaleResult> Handle(
        GetSaleByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingSaleRetrieval(request.SaleId);

        var saleId = SaleId.Create(request.SaleId);

        var sale = await _saleRepository.FindByIdAsync(saleId, cancellationToken);

        if (sale == null)
        {
            LogSaleNotFound();

            throw new SaleNotFoundException(saleId);
        }

        LogSaleRetrievedSuccessfully(request.SaleId);

        return SaleResult.FromSale(sale);
    }
}
