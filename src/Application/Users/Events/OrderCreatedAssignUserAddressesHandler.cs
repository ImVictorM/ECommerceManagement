using Application.Common.Interfaces.Persistence;

using Domain.OrderAggregate.Events;
using Domain.UserAggregate.Specification;

using MediatR;

namespace Application.Users.Events;

/// <summary>
/// Handles the <see cref="OrderCreated"/> event
/// by assigning the delivery and billing addresses to the user.
/// </summary>
public class OrderCreatedAssignUserAddressesHandler : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedAssignUserAddressesHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderCreatedAssignUserAddressesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository
            .FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(notification.Order.OwnerId));

        if (user != null)
        {
            user.AssignAddress(notification.Order.DeliveryAddress, notification.BillingAddress);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
