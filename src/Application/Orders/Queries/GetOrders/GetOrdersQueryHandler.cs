using Application.Orders.DTOs;
using Application.Common.Persistence;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Specifications;

using SharedKernel.Models;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetOrders;

/// <summary>
/// Handles the <see cref="GetOrdersQuery"/> query by
/// retrieving all the orders.
/// The orders can be filtered by status.
/// </summary>
public sealed partial class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrdersQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetOrdersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetOrdersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingOrdersRetrieval(request.Status);

        var statusFilter = request.Status != null ?
            BaseEnumeration.FromDisplayName<OrderStatus>(request.Status)
            : null;

        var orders = await _unitOfWork.OrderRepository.FindSatisfyingAsync(new QueryOrderByStatusSpecification(statusFilter));

        LogOrdersRetrievedSuccessfully(orders.Count());

        return orders.Select(order => new OrderResult(order));
    }
}
