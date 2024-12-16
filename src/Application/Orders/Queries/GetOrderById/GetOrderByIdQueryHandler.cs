using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Common.DTOs;
using Application.Orders.Common.Errors;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Application.Orders.Queries.GetOrderById;

/// <summary>
/// Query handler for the <see cref="GetOrderByIdQuery"/> query.
/// </summary>
public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderAccessServices _orderAccessServices;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="orderAccessServices">The order access services.</param>
    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IOrderAccessServices orderAccessServices)
    {
        _unitOfWork = unitOfWork;
        _orderAccessServices = orderAccessServices;
    }

    /// <inheritdoc/>
    public async Task<OrderResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Create(request.OrderId);
        var currentUserId = UserId.Create(request.CurrentUserId);

        var order = await _unitOfWork.OrderRepository.FindByIdAsync(orderId) ?? throw new OrderNotFoundException();

        if (!await _orderAccessServices.CanUserReadOrder(order, currentUserId))
        {
            throw new UserNotAllowedException($"The current user does not have permission to access the order with id {orderId}");
        }

        return new OrderResult(order);
    }
}
