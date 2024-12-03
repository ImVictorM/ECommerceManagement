using Application.Common.Interfaces.Persistence;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Infrastructure.Persistence;

/// <summary>
/// Defines the component used for atomic operation between repositories.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ECommerceDbContext _context;

    /// <inheritdoc/>
    public IRepository<User, UserId> UserRepository { get; }

    /// <inheritdoc/>
    public IRepository<Product, ProductId> ProductRepository { get; }

    /// <inheritdoc/>
    public IRepository<Order, OrderId> OrderRepository { get; }

    /// <inheritdoc/>
    public IRepository<Payment, PaymentId> PaymentRepository { get; }



    /// <summary>
    /// Initiates a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="paymentRepository">The payment repository.</param>
    public UnitOfWork(
        ECommerceDbContext context,
        IRepository<User, UserId> userRepository,
        IRepository<Product, ProductId> productRepository,
        IRepository<Order, OrderId> orderRepository,
        IRepository<Payment, PaymentId> paymentRepository
    )
    {
        _context = context;

        UserRepository = userRepository;
        ProductRepository = productRepository;
        OrderRepository = orderRepository;
        PaymentRepository = paymentRepository;
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _context.Dispose();
    }
}
