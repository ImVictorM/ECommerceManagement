using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.InstallmentAggregate;
using Domain.OrderAggregate;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductFeedbackAggregate;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.Entities;
using Domain.UserAggregate;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence;

/// <summary>
/// The application db context.
/// </summary>
public class ECommerceDbContext : DbContext
{
    /// <summary>
    /// The prefix used in the database PKs and FKs.
    /// By convention, all the PKs will have this exactly name.
    /// </summary>
    private const string KeyPrefix = "id";

    /// <summary>
    /// The context interceptors.
    /// </summary>
    private readonly IEnumerable<IInterceptor> _interceptors;

    /// <summary>
    /// Gets or sets the user aggregate context.
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    /// Gets or sets the installment aggregate context.
    /// </summary>
    public DbSet<Installment> Installments { get; set; }
    /// <summary>
    /// Gets or sets the order aggregate context.
    /// </summary>
    public DbSet<Order> Orders { get; set; }
    /// <summary>
    /// Gets or sets the payment aggregate context.
    /// </summary>
    public DbSet<Payment> Payments { get; set; }
    /// <summary>
    /// Gets or sets the payment method aggregate context.
    /// </summary>
    public DbSet<PaymentMethod> PaymentsMethods { get; set; }
    /// <summary>
    /// Gets or sets the payment status aggregate context.
    /// </summary>
    public DbSet<PaymentStatus> PaymentStatuses { get; set; }
    /// <summary>
    /// Gets or sets the product aggregate context.
    /// </summary>
    public DbSet<Product> Products { get; set; }
    /// <summary>
    /// Gets or sets the product feedback aggregate context.
    /// </summary>
    public DbSet<ProductFeedback> ProductFeedbacks { get; set; }
    /// <summary>
    /// Gets or sets the shipment aggregate context.
    /// </summary>
    public DbSet<Shipment> Shipments { get; set; }
    /// <summary>
    /// Gets or sets the shipment status aggregate context.
    /// </summary>
    public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }

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

    /// <summary>
    /// Normalize the database column names to be snake case and lower case.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
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

        var snakeCaseColumnName = ConvertToSnakeCase(property.Name);

        if (snakeCaseColumnName.EndsWith($"_{KeyPrefix}", StringComparison.OrdinalIgnoreCase))
        {
            // Invert the order in case it ends with _id
            snakeCaseColumnName = $"{KeyPrefix}_{snakeCaseColumnName[..(snakeCaseColumnName.Length - 3)]}";
        }

        property.SetColumnName(snakeCaseColumnName);
    }

    /// <summary>
    /// Determines if the property is a shadow primary key of an owned type.
    /// </summary>
    /// <param name="p">The property to be checked.</param>
    /// <returns>A boolean indicating if the property is a shadow PK of an owned type.</returns>
    private static bool IsShadowPrimaryKeyOfOwnedType(IMutableProperty p)
    {
        return typeof(ValueObject).IsAssignableFrom(p.DeclaringType.ClrType) && p.IsKey();
    }

    /// <summary>
    /// Converts a string to snake case.
    /// </summary>
    /// <param name="str">The string to be converted</param>
    /// <returns>A snake case string.</returns>
    private static string ConvertToSnakeCase(string str)
    {
        return string.Concat(
            str.Select((character, index) => index > 0 && char.IsUpper(character) ? $"_{character}" : $"{character}")
        ).ToLowerInvariant();
    }
}
