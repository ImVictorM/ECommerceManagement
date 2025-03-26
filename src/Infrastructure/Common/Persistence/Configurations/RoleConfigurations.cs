using Infrastructure.Common.Persistence.Configurations.Abstracts;

using SharedKernel.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

internal sealed class RoleConfigurations
    : EntityTypeConfigurationDependency<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
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
