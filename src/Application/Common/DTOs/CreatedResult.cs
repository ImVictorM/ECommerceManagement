namespace Application.Common.DTOs;

/// <summary>
/// Represents a created response that returns the created resource's identifier.
/// </summary>
/// <param name="Id">The created resource identifier.</param>
public record CreatedResult(string Id);
