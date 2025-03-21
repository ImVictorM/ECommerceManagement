using Application.Common.Persistence.Repositories;
using Application.Sales.DTOs;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Sales.Queries.GetSales;

internal sealed partial class GetSalesQueryHandler
    : IRequestHandler<GetSalesQuery, IReadOnlyList<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesQueryHandler(
        ISaleRepository saleRepository,
        ILogger<GetSalesQueryHandler> logger
    )
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<SaleResult>> Handle(
        GetSalesQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingSalesRetrieval(
            request.Filters.ExpiringAfter,
            request.Filters.ExpiringBefore,
            request.Filters.ValidForDate
        );

        var sales = await _saleRepository.GetSalesAsync(
            request.Filters,
            cancellationToken
        );

        LogSalesRetrievedSuccessfully(sales.Count);

        return sales
            .Select(sale => new SaleResult(sale))
            .ToList();
    }
}
