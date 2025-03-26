namespace Application.Common.DTOs.Results;

/// <summary>
/// Represents a created result that contains the created resource's identifier.
/// </summary>
/// <param name="Id">The created resource identifier.</param>
public record CreatedResult(string Id);
