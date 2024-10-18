using Domain.UserAggregate.Entities;
using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.UserAggregate;

/// <summary>
/// Configures the <see cref="Role"/> entity to its table.
/// </summary>
public sealed class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id);

        builder
            .Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => RoleId.Create(value)
            )
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(r => r.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasData(Role.List());
    }
}
