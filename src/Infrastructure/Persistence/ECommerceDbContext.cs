using Domain.AddressAggregate;
using Domain.DiscountAggregate;
using Domain.InstallmentAggregate;
using Domain.OrderAggregate;
using Domain.OrderStatusAggregate;
using Domain.PaymentAggregate;
using Domain.PaymentMethodAggregate;
using Domain.PaymentStatusAggregate;
using Domain.ProductAggregate;
using Domain.ProductCategoryAggregate;
using Domain.ProductFeedbackAggregate;
using Domain.RoleAggregate;
using Domain.ShipmentAggregate;
using Domain.ShipmentStatusAggregate;
using Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// The application db context.
/// </summary>
public class ECommerceDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the user aggregate context.
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    /// Gets or sets the address aggregate context.
    /// </summary>
    public DbSet<Address> Addresses { get; set; }
    /// <summary>
    /// Gets or sets the role aggregate context.
    /// </summary>
    public DbSet<Role> Roles { get; set; }
    /// <summary>
    /// Gets or sets the discount aggregate context.
    /// </summary>
    public DbSet<Discount> Discounts { get; set; }
    /// <summary>
    /// Gets or sets the installment aggregate context.
    /// </summary>
    public DbSet<Installment> Installments { get; set; }
    /// <summary>
    /// Gets or sets the order aggregate context.
    /// </summary>
    public DbSet<Order> Orders { get; set; }
    /// <summary>
    /// Gets or sets the order status aggregate context.
    /// </summary>
    public DbSet<OrderStatus> OrderStatuses { get; set; }
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
    /// Gets or sets the product category aggregate context.
    /// </summary>
    public DbSet<ProductCategory> ProductCategories { get; set; }
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
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);

        NormalizeColumnNames(modelBuilder);

        base.OnModelCreating(modelBuilder);
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
            .ForEach(p =>
            {
                // Column name to snake case
                var snakeCaseColumnName = string
                    .Concat(
                        p.Name.Select((character, index) => index > 0 && char.IsUpper(character) ? $"_{character}" : $"{character}")
                    )
                    .ToLowerInvariant();

                // Invert the order in case it ends with _id
                if (snakeCaseColumnName.EndsWith("_id", StringComparison.OrdinalIgnoreCase))
                {
                    snakeCaseColumnName = $"id_{snakeCaseColumnName[..(snakeCaseColumnName.Length - 3)]}";
                }

                p.SetColumnName(snakeCaseColumnName);
            });
    }
}
