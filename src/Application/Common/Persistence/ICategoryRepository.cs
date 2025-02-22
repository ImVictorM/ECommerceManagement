using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

namespace Application.Common.Persistence;

/// <summary>
/// Defines the contract for category persistence operations.
/// </summary>
public interface ICategoryRepository : IBaseRepository<Category, CategoryId>
{
}
