namespace Application.Orders.Commands.Common.DTOs;

/// <summary>
/// Represents a product input DTO.
/// </summary>
/// <param name="Id">The product id.</param>
/// <param name="Quantity">The product quantity.</param>
public record OrderProductInput(string Id, int Quantity);
