using Domain.CategoryAggregate;

namespace Application.Categories.DTOs.Results;

/// <summary>
/// Represents a category result.
/// </summary>
public class CategoryResult
{
    /// <summary>
    /// Gets the category identifier.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the category name.
    /// </summary>
    public string Name { get; }

    private CategoryResult(Category category)
    {
        Id = category.Id.ToString();
        Name = category.Name;
    }

    internal static CategoryResult FromCategory(Category category)
    {
        return new CategoryResult(category);
    }
};
