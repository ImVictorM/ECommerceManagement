using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;

namespace Application.Common.Interfaces.Persistence;

/// <summary>
/// The component used for atomic operation between repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the user repository.
    /// </summary>
    public IRepository<User, UserId> UserRepository { get; }
    /// <summary>
    /// Gets the product repository.
    /// </summary>
    public IRepository<Product, ProductId> ProductRepository { get; }
    /// <summary>
    /// Gets the order repository.
    /// </summary>
    public IRepository<Order, OrderId> OrderRepository { get; }
    /// <summary>
    /// Gets the payment repository.
    /// </summary>
    public IRepository<Payment, PaymentId> PaymentRepository { get; }
    /// <summary>
    /// Save all the operations done within the repositories.
    /// </summary>
    /// <returns>An asynchronous operation containing the number of entries modified.</returns>
    Task<int> SaveChangesAsync();
}
