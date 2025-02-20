using Application.Orders.DTOs;
using Application.Common.Persistence;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Specifications;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.Queries.GetOrders;

/// <summary>
/// Handles the <see cref="GetOrdersQuery"/> query by
/// retrieving all the orders.
/// The orders can be filtered by status.
/// </summary>
public sealed partial class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderResult>>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrdersQueryHandler"/> class.
    /// </summary>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="logger">The logger.</param>
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
