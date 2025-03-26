using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Categories;

internal sealed class CategoryConfigurations
    : EntityTypeConfigurationDependency<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
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
