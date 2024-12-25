using Application.Common.Interfaces.Persistence;

using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
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

    /// <inheritdoc/>
    public IRepository<Category, CategoryId> CategoryRepository { get; }

    /// <inheritdoc/>
    public IRepository<Sale, SaleId> SaleRepository { get; }

    /// <inheritdoc/>
    public IRepository<Coupon, CouponId> CouponRepository { get; }


    /// <summary>
    /// Initiates a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="paymentRepository">The payment repository.</param>
    /// <param name="categoryRepository">The category repository.</param>
    /// <param name="saleRepository">The sale repository.</param>
    /// <param name="couponRepository">The coupon repository.</param>
    public UnitOfWork(
        ECommerceDbContext context,
        IRepository<User, UserId> userRepository,
        IRepository<Product, ProductId> productRepository,
        IRepository<Order, OrderId> orderRepository,
        IRepository<Payment, PaymentId> paymentRepository,
        IRepository<Category, CategoryId> categoryRepository,
        IRepository<Sale, SaleId> saleRepository,
        IRepository<Coupon, CouponId> couponRepository
    )
    {
        _context = context;

        UserRepository = userRepository;
        ProductRepository = productRepository;
        OrderRepository = orderRepository;
        PaymentRepository = paymentRepository;
        CategoryRepository = categoryRepository;
        SaleRepository = saleRepository;
        CouponRepository = couponRepository;
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
