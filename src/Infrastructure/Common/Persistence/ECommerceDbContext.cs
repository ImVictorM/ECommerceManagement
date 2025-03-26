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

internal sealed class ECommerceDbContext : DbContext, IECommerceDbContext
{
    private const string KeyPrefix = "id";
    private readonly IEnumerable<IInterceptor> _interceptors;
    private readonly IEnumerable<EntityTypeConfigurationDependency> _configurations;

    public DbSet<Category> Categories { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<DomainProductFeedback> ProductFeedback { get; set; }

    public DbSet<Shipment> Shipments { get; set; }

    public DbSet<Coupon> Coupons { get; set; }

    public DbSet<CouponRestriction> CouponRestrictions { get; set; }

    public DbSet<Sale> Sales { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Carrier> Carriers { get; set; }

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


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptors);

        base.OnConfiguring(optionsBuilder);
    }

    private static void NormalizeColumnNames(ModelBuilder modelBuilder)
    {

        modelBuilder.Model
            .GetEntityTypes()
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

        var endingWithPrefix = $"_{KeyPrefix}";

        if (snakeCaseColumnName.EndsWith(
            endingWithPrefix,
            StringComparison.OrdinalIgnoreCase
        ))
        {
            // Invert the order in case it ends with _id

            var baseName = snakeCaseColumnName[..^endingWithPrefix.Length];

            snakeCaseColumnName = $"{KeyPrefix}_{baseName}";
        }

        property.SetColumnName(snakeCaseColumnName);
    }

    private static bool IsShadowPrimaryKeyOfOwnedType(IMutableProperty p)
    {
        return
            typeof(ValueObject).IsAssignableFrom(p.DeclaringType.ClrType)
            && p.IsKey();
    }
}
