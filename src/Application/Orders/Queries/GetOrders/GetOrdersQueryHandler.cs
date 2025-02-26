using Application.Common.Persistence.Repositories;
using Application.Orders.DTOs;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Specifications;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.Queries.GetOrders;

internal sealed partial class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderResult>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository, ILogger<GetOrdersQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingOrdersRetrieval(request.Status);

        var statusFilter = request.Status != null ?
            BaseEnumeration.FromDisplayName<OrderStatus>(request.Status)
            : null;

        var orders = await _orderRepository.FindSatisfyingAsync(
            new QueryOrderByStatusSpecification(statusFilter),
            cancellationToken
        );

        LogOrdersRetrievedSuccessfully(orders.Count());

        return orders.Select(order => new OrderResult(order));
    }
}
