using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Common.DTOs;
using Application.Orders.Common.Errors;

using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using MediatR;

namespace Application.Orders.Queries.GetOrderById;

/// <summary>
/// Query handler for the <see cref="GetOrderByIdQuery"/> query.
/// </summary>
public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailedResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<OrderDetailedResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Create(request.OrderId);
        var currentUserId = UserId.Create(request.CurrentUserId);

        var order =
            await _unitOfWork.OrderRepository.FindByIdAsync(orderId)
            ?? throw new OrderNotFoundException();

        var currentUser = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(currentUserId));

        if (currentUser == null || !new ReadOrderSpecification(order.OwnerId).IsSatisfiedBy(currentUser))
        {
            throw new UserNotAllowedException($"The current user does not have permission to access the order with id {orderId}");
        }

        var orderPayment = await _unitOfWork.PaymentRepository.FindOneOrDefaultAsync(p => p.OrderId == order.Id);

        return new OrderDetailedResult(order, orderPayment);
    }
}
