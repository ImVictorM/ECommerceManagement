using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for category persistence operations.
/// </summary>
public interface ICategoryRepository : IBaseRepository<Category, CategoryId>
{
}
