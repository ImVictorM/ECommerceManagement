using Application.Orders.DTOs;
using Application.Common.Persistence;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Specifications;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Handles the <see cref="GetCustomerOrdersQuery"/> query by
/// fetching all orders of a customer.
/// </summary>
public sealed partial class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, IEnumerable<OrderResult>>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrdersQueryHandler"/> class.
    /// </summary>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="logger">The logger.</param>
    public GetCustomerOrdersQueryHandler(IOrderRepository orderRepository, ILogger<GetCustomerOrdersQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderResult>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingOrdersRetrieval(request.UserId);

        var orderOwnerId = UserId.Create(request.UserId);
        var statusFilterCondition = request.Status is not null
            ? BaseEnumeration.FromDisplayName<OrderStatus>(request.Status)
            : null;

        var specifications =
            new QueryOrderByStatusSpecification(statusFilterCondition)
            .And(new QueryCustomerOrderSpecification(orderOwnerId));

        var orders = await _orderRepository.FindSatisfyingAsync(specifications, cancellationToken);

        LogOrdersRetrievedSuccessfully(orders.Count());

        return orders.Select(order => new OrderResult(order));
    }
}
