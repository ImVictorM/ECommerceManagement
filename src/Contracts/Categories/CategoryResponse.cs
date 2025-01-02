namespace Contracts.Categories;

/// <summary>
/// Represents a category response.
/// </summary>
/// <param name="Id">The category id.</param>
/// <param name="Name">The category name.</param>
public record CategoryResponse(
    string Id,
    string Name
);
