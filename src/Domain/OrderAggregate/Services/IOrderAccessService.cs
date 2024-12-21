using Domain.UserAggregate.ValueObjects;

namespace Domain.OrderAggregate.Services;

/// <summary>
/// Services to manage order access.
/// </summary>
public interface IOrderAccessService
{
    /// <summary>
    /// Indicates if a user can read an order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A boolean value indicating if a user can read the order.</returns>
    Task<bool> CanUserReadOrder(Order order, UserId userId);
}
