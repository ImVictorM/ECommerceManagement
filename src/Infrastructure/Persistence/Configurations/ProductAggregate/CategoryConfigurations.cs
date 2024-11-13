using Domain.ProductAggregate.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ProductAggregate;

/// <summary>
/// Configures the <see cref="Category"/> value object to its table.
/// </summary>
public sealed class CategoryConfigurations : IEntityTypeConfiguration<Category>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(c => c.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasData(Category.List());
    }
}
