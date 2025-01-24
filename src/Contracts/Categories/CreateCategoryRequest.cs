namespace Contracts.Categories;

/// <summary>
/// Represents a request to create a new category.
/// </summary>
/// <param name="Name">The category name.</param>
public record CreateCategoryRequest(
    string Name
);
