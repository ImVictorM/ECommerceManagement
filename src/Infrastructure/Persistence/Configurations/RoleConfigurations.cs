using Domain.RoleAggregate;
using Domain.RoleAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="Role"/> aggregate to entity framework.
/// </summary>
public sealed class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        ConfigureRoleTable(builder);
    }

    /// <summary>
    /// Configure the role table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureRoleTable(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(role => role.Id);

        builder
            .Property(role => role.Id)
            .HasConversion(
                roleId => roleId.Value,
                value => RoleId.Create(value)
            );

        builder
            .Property(role => role.Name)
            .HasMaxLength(120)
            .IsRequired();
    }
}
