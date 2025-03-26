namespace Application.Orders.DTOs.Filters;

/// <summary>
/// Represents filtering criteria for order queries.
/// </summary>
/// <param name="Status">
/// Filters orders that matches the specified status.
/// </param>
public record OrderFilters(string? Status = null);
