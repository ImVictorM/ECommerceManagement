using Domain.ShippingMethodAggregate;

namespace Application.ShippingMethods.DTOs;

/// <summary>
/// Represents a shipping method result.
/// </summary>
/// <param name="ShippingMethod">The shipping method.</param>
public record ShippingMethodResult(ShippingMethod ShippingMethod);
