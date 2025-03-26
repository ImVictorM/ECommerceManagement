using Application.Common.Persistence.Repositories;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.Orders.DTOs.Results;

namespace Application.Orders.Queries.GetOrders;

internal sealed partial class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderResult>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        ILogger<GetOrdersQueryHandler> logger
    )
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<OrderResult>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingOrdersRetrieval(request.Filters.Status);

        var orders = await _orderRepository.GetOrdersAsync(
            request.Filters,
            cancellationToken
        );

        LogOrdersRetrievedSuccessfully(orders.Count);

        return orders
            .Select(OrderResult.FromProjection)
            .ToList();
    }
}
