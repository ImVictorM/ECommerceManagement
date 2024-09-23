using Application;
using Domain.AddressAggregate;
using Domain.RoleAggregate;
using Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ECommerceDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Role> Roles { get; set; }

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
