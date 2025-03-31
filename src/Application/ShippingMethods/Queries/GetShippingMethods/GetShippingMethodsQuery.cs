using Application.ShippingMethods.DTOs.Results;

using MediatR;

namespace Application.ShippingMethods.Queries.GetShippingMethods;

/// <summary>
/// Represents a query to retrieve all shipping methods.
/// </summary>
public class GetShippingMethodsQuery
    : IRequest<IReadOnlyList<ShippingMethodResult>>;
