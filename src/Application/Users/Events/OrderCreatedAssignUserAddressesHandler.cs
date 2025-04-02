using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.OrderAggregate.Events;
using Domain.UserAggregate.Specification;

using MediatR;

namespace Application.Users.Events;

internal sealed class OrderCreatedAssignUserAddressesHandler
    : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public OrderCreatedAssignUserAddressesHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository
    )
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(
        OrderCreated notification,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(notification.Order.OwnerId),
            cancellationToken
        );

        if (user == null)
        {
            return;
        }

        user.AssignAddress(
            notification.DeliveryAddress,
            notification.BillingAddress
        );

        await _unitOfWork.SaveChangesAsync();
    }
}
