using Application.Common.Security.Authorization.Roles;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Security.Authorization.Roles;

/// <summary>
///  Configures the <see cref="Role"/> table.
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
