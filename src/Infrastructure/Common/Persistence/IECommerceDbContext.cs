using Domain.CategoryAggregate;
using Domain.CouponAggregate;
using Domain.CouponAggregate.Abstracts;
using Domain.OrderAggregate;
using Domain.PaymentAggregate;
using Domain.ProductAggregate;
using Domain.SaleAggregate;
using Domain.ShipmentAggregate;
using Domain.UserAggregate;
using Domain.CarrierAggregate;
using Domain.ShippingMethodAggregate;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Common.Persistence;

/// <summary>
/// Represents the application database context.
/// </summary>
public interface IECommerceDbContext : IDisposable
{
    /// <summary>
    /// Gets or sets the category aggregate context.
    /// </summary>
    public DbSet<Category> Categories { get; set; }
    /// <summary>
    /// Gets or sets the user aggregate context.
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    /// Gets or sets the order aggregate context.
    /// </summary>
    public DbSet<Order> Orders { get; set; }
    /// <summary>
    /// Gets or sets the product aggregate context.
    /// </summary>
    public DbSet<Product> Products { get; set; }
    /// <summary>
    /// Gets or sets the product feedback aggregate context.
    /// </summary>
    public DbSet<DomainProductFeedback> ProductFeedback { get; set; }
    /// <summary>
    /// Gets or sets the shipment aggregate context.
    /// </summary>
    public DbSet<Shipment> Shipments { get; set; }
    /// <summary>
    /// Gets or sets the coupons aggregate context.
    /// </summary>
    public DbSet<Coupon> Coupons { get; set; }
    /// <summary>
    /// Gets or sets the coupon restrictions aggregate context.
    /// </summary>
    public DbSet<CouponRestriction> CouponRestrictions { get; set; }
    /// <summary>
    /// Gets or sets the sale aggregate context.
    /// </summary>
    public DbSet<Sale> Sales { get; set; }
    /// <summary>
    /// Gets or sets the payment aggregate context.
    /// </summary>
    public DbSet<Payment> Payments { get; set; }
    /// <summary>
    /// Gets or sets the carrier aggregate context.
    /// </summary>
    public DbSet<Carrier> Carriers { get; set; }
    /// <summary>
    /// Gets or sets the shipping method aggregate context.
    /// </summary>
    public DbSet<ShippingMethod> ShippingMethods { get; set; }

    /// <summary>
    /// Gets the database related information and operations for this context.
    /// </summary>
    public DatabaseFacade Database { get; }

    /// <summary>
    /// Commits all pending changes within the current transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the number of affected entries.
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an <see cref="EntityEntry{TEntity}" /> for the given entity. The entry provides
    /// access to change tracking information and operations for the entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to get the entry for.</param>
    /// <returns>The entry for the given entity.</returns>
    public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

#pragma warning disable CA1716
    /// <summary>
    /// Provides access to the <see cref="DbSet{TEntity}"/> for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <returns>A <see cref="DbSet{TEntity}"/> for the given entity type.</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
#pragma warning restore CA1716
}
