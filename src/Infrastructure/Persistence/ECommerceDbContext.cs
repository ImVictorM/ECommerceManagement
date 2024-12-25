using Domain.CouponAggregate;
using Domain.CouponAggregate.Abstracts;
using Domain.OrderAggregate;
using Domain.PaymentAggregate;
using Domain.ProductAggregate;
using Domain.ProductFeedbackAggregate;
using Domain.SaleAggregate;
using Domain.ShipmentAggregate;
using Domain.UserAggregate;

using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Infrastructure.Persistence;

/// <summary>
/// The application db context.
/// </summary>
public class ECommerceDbContext : DbContext
{
    private const string KeyPrefix = "id";
    private readonly IEnumerable<IInterceptor> _interceptors;

    /// <summary>
    /// Gets or sets the user aggregate context.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;
    /// <summary>
    /// Gets or sets the order aggregate context.
    /// </summary>
    public DbSet<Order> Orders { get; set; } = null!;
    /// <summary>
    /// Gets or sets the payment aggregate context.
    /// </summary>
    public DbSet<Payment> Payments { get; set; } = null!;
    /// <summary>
    /// Gets or sets the product aggregate context.
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;
    /// <summary>
    /// Gets or sets the product feedback aggregate context.
    /// </summary>
    public DbSet<ProductFeedback> ProductFeedbacks { get; set; } = null!;
    /// <summary>
    /// Gets or sets the shipment aggregate context.
    /// </summary>
    public DbSet<Shipment> Shipments { get; set; } = null!;
    /// <summary>
    /// Gets or sets the coupons aggregate context.
    /// </summary>
    public DbSet<Coupon> Coupons { get; set; } = null!;
    /// <summary>
    /// Gets or sets the coupon restrictions aggregate context.
    /// </summary>
    public DbSet<CouponRestriction> CouponRestrictions { get; set; } = null!;
    /// <summary>
    /// Gets or sets the sale aggregate context.
    /// </summary>
    public DbSet<Sale> Sales { get; set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="ECommerceDbContext"/> class.
    /// </summary>
    /// <param name="options">The db context options.</param>
    /// <param name="auditInterceptor">The audit interceptor.</param>
    /// <param name="publishDomainEventInterceptor">The publish domain events interceptor.</param>
    public ECommerceDbContext(
        DbContextOptions<ECommerceDbContext> options,
        AuditInterceptor auditInterceptor,
        PublishDomainEventsInterceptor publishDomainEventInterceptor
    )
        : base(options)
    {
        _interceptors = [auditInterceptor, publishDomainEventInterceptor];
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);

        NormalizeColumnNames(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptors);

        base.OnConfiguring(optionsBuilder);
    }

    private static void NormalizeColumnNames(ModelBuilder modelBuilder)
    {

        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .ToList()
            .ForEach(NormalizePropertyColumnName);
    }

    private static void NormalizePropertyColumnName(IMutableProperty property)
    {
        if (IsShadowPrimaryKeyOfOwnedType(property))
        {
            // Set column name to match the owner primary key.
            property.SetColumnName(KeyPrefix);
            return;
        }

        var snakeCaseColumnName = property.Name.ToLowerSnakeCase().Trim('_');

        if (snakeCaseColumnName.EndsWith($"_{KeyPrefix}", StringComparison.OrdinalIgnoreCase))
        {
            // Invert the order in case it ends with _id
            snakeCaseColumnName = $"{KeyPrefix}_{snakeCaseColumnName[..(snakeCaseColumnName.Length - 3)]}";
        }

        property.SetColumnName(snakeCaseColumnName);
    }

    private static bool IsShadowPrimaryKeyOfOwnedType(IMutableProperty p)
    {
        return typeof(ValueObject).IsAssignableFrom(p.DeclaringType.ClrType) && p.IsKey();
    }
}
