using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;

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
    /// Gets the category repository.
    /// </summary>
    public IRepository<Category, CategoryId> CategoryRepository { get; }
    /// <summary>
    /// Gets the sale repository.
    /// </summary>
    public IRepository<Sale, SaleId> SaleRepository { get; }
    /// <summary>
    /// Gets the coupon repository.
    /// </summary>
    public IRepository<Coupon, CouponId> CouponRepository { get; }
    /// <summary>
    /// Gets the payment repository.
    /// </summary>
    public IRepository<Payment, PaymentId> PaymentRepository { get; }
    /// <summary>
    /// Gets the shipment repository.
    /// </summary>
    public IRepository<Shipment, ShipmentId> ShipmentRepository { get; }
    /// <summary>
    /// Save all the operations done within the repositories.
    /// </summary>
    /// <returns>An asynchronous operation containing the number of entries modified.</returns>
    Task<int> SaveChangesAsync();
}
