using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations.Abstracts;

/// <summary>
/// Configures the entities with dependency injection support.
/// </summary>
public abstract class EntityTypeConfigurationDependency
{
    /// <summary>
    /// Configures the entities through the model builder.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    public abstract void Configure(ModelBuilder modelBuilder);
}

/// <summary>
/// Configures the entities with dependency injection support.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class EntityTypeConfigurationDependency<TEntity>
    : EntityTypeConfigurationDependency, IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    /// <inheritdoc/>
    public abstract void Configure(EntityTypeBuilder<TEntity> builder);

    /// <inheritdoc/>
    public override void Configure(ModelBuilder modelBuilder) => Configure(modelBuilder.Entity<TEntity>());
}
