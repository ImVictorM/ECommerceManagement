using Application.ShippingMethods.DTOs;

using MediatR;

namespace Application.ShippingMethods.Queries.GetShippingMethodById;

/// <summary>
/// Represents a query to get a shipping method by id.
/// </summary>
/// <param name="ShippingMethodId">The shipping method.</param>
public record GetShippingMethodByIdQuery(string ShippingMethodId) : IRequest<ShippingMethodResult>;
