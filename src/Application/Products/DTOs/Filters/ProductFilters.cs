namespace Application.Products.DTOs.Filters;

/// <summary>
/// Represents filtering criteria for products queries.
/// </summary>
/// <param name="CategoryIds">
/// Filters products that belongs to all the specified category identifiers.
/// </param>
public record ProductFilters(IEnumerable<string> CategoryIds);
