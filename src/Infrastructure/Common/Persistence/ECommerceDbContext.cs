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

using Infrastructure.Common.Persistence.Interceptors;
using Infrastructure.Common.Persistence.Configurations.Abstracts;

using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using SharedKernel.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Common.Persistence;

internal sealed class ECommerceDbContext : DbContext
{
    private const string KeyPrefix = "id";
    private readonly IEnumerable<IInterceptor> _interceptors;
    private readonly IEnumerable<EntityTypeConfigurationDependency> _configurations;

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

    public ECommerceDbContext(
        DbContextOptions<ECommerceDbContext> options,
        AuditInterceptor auditInterceptor,
        PublishDomainEventsInterceptor publishDomainEventInterceptor,
        IEnumerable<EntityTypeConfigurationDependency> configurations
    )
        : base(options)
    {
        _interceptors = [auditInterceptor, publishDomainEventInterceptor];
        _configurations = configurations;
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<List<IDomainEvent>>();

        foreach (var entityTypeConfiguration in _configurations)
        {
            entityTypeConfiguration.Configure(modelBuilder);
        }

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
