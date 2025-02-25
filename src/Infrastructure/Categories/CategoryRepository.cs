using Application.Common.Persistence.Repositories;

using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Categories;

/// <summary>
/// Defines the implementation for category persistence operations.
/// </summary>
public sealed class CategoryRepository : BaseRepository<Category, CategoryId>, ICategoryRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CategoryRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public CategoryRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
