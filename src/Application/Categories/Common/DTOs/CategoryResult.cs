namespace Application.Categories.Common.DTOs;

/// <summary>
/// Represents a category result DTO.
/// </summary>
/// <param name="Id">The category id.</param>
/// <param name="Name">The category name.</param>
public record class CategoryResult(string Id, string Name);
