using Domain.AddressAggregate;
using Domain.RoleAggregate;
using Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// The application db context.
/// </summary>
public class ECommerceDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Role> Roles { get; set; }

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
