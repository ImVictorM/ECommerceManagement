using Application.Common.Persistence.Repositories;

using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.Orders.DTOs.Results;

namespace Application.Orders.Queries.GetCustomerOrders;

internal sealed partial class GetCustomerOrdersQueryHandler
    : IRequestHandler<GetCustomerOrdersQuery, IReadOnlyList<OrderResult>>
{
    private readonly IOrderRepository _orderRepository;

    public GetCustomerOrdersQueryHandler(
        IOrderRepository orderRepository,
        ILogger<GetCustomerOrdersQueryHandler> logger
    )
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<OrderResult>> Handle(
        GetCustomerOrdersQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerOrdersRetrieval(
            request.OwnerId,
            request.Filters.Status
        );

        var ownerId = UserId.Create(request.OwnerId);

        var orders = await _orderRepository.GetCustomerOrdersAsync(
            ownerId,
            request.Filters,
            cancellationToken
        );

        LogOrdersRetrievedSuccessfully(orders.Count);

        return orders
            .Select(OrderResult.FromProjection)
            .ToList();
    }
}
