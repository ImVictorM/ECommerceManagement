namespace Contracts.Categories;

/// <summary>
/// Represents a category response.
/// </summary>
/// <param name="Id">The category identifier.</param>
/// <param name="Name">The category name.</param>
public record CategoryResponse(
    string Id,
    string Name
);
