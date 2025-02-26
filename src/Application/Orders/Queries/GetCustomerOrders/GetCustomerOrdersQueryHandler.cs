using Application.Common.Persistence.Repositories;
using Application.Orders.DTOs;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Specifications;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.Queries.GetCustomerOrders;

internal sealed partial class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, IEnumerable<OrderResult>>
{
    private readonly IOrderRepository _orderRepository;

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
