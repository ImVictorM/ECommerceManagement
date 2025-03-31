using Application.ShippingMethods.DTOs.Results;

using MediatR;

namespace Application.ShippingMethods.Queries.GetShippingMethodById;

/// <summary>
/// Represents a query to get a shipping method by its identifier.
/// </summary>
/// <param name="ShippingMethodId">The shipping method identifier.</param>
public record GetShippingMethodByIdQuery(string ShippingMethodId)
    : IRequest<ShippingMethodResult>;
