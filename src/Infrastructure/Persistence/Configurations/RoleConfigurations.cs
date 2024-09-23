using Domain.RoleAggregate;
using Domain.RoleAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        ConfigureRole(builder);
    }

    private static void ConfigureRole(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(role => role.Id).HasName(nameof(Role.Id).ToLowerInvariant());

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
