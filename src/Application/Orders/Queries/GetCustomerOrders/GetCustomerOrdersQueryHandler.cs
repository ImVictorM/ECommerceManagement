using Application.Common.Persistence;
using Application.Orders.Common.DTOs;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Specifications;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Models;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Handles the <see cref="GetCustomerOrdersQuery"/> query by
/// fetching all orders of a customer.
/// </summary>
public sealed partial class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, IEnumerable<OrderResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrdersQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetCustomerOrdersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCustomerOrdersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
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

        var specifications = new QueryOrderByStatusSpecification(statusFilterCondition).And(new QueryCustomerOrderSpecification(orderOwnerId));

        var orders = await _unitOfWork.OrderRepository.FindSatisfyingAsync(specifications);

        LogOrdersRetrievedSuccessfully(orders.Count());

        return orders.Select(order => new OrderResult(order));
    }
}
