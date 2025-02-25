using Application.Common.Persistence.Repositories;

using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Categories;

internal sealed class CategoryRepository : BaseRepository<Category, CategoryId>, ICategoryRepository
{
    public CategoryRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
