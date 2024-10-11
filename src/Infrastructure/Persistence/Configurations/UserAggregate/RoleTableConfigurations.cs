using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.UserAggregate;

/// <summary>
/// Configures the <see cref="Role"/> value object to its table.
/// </summary>
public sealed class RoleTableConfigurations : IEntityTypeConfiguration<Role>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .Property(role => role.Name)
            .HasMaxLength(120)
            .IsRequired();
    }
}
