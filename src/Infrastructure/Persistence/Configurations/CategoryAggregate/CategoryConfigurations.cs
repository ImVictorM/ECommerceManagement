using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.CategoryAggregate;

/// <summary>
/// Configures the tables for the <see cref="Category"/> root.
/// </summary>
public class CategoryConfigurations : IEntityTypeConfiguration<Category>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .HasConversion(id => id.Value, value => CategoryId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(c => c.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
