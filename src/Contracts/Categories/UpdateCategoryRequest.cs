namespace Contracts.Categories;

/// <summary>
/// Represents a request to update an existing category.
/// </summary>
/// <param name="Name">The new category name.</param>
public record UpdateCategoryRequest(
    string Name
);
