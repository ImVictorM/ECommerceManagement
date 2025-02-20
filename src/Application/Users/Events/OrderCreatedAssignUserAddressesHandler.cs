using Application.Common.Persistence;

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
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedAssignUserAddressesHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userRepository">The user repository.</param>
    public OrderCreatedAssignUserAddressesHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository
    )
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(notification.Order.OwnerId),
            cancellationToken
        );

        if (user != null)
        {
            user.AssignAddress(notification.DeliveryAddress, notification.BillingAddress);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
