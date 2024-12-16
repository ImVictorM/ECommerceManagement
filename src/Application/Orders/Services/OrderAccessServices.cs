using Application.Common.Interfaces.Persistence;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Services;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

namespace Application.Orders.Services;

/// <summary>
/// Services to manage order access.
/// </summary>
public class OrderAccessServices : IOrderAccessServices
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new intance of the <see cref="OrderAccessServices"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderAccessServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<bool> CanUserReadOrder(Order order, UserId userId)
    {
        var currentUser = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(userId));

        return order.OwnerId == userId || (currentUser != null && currentUser.IsAdmin());
    }
}
