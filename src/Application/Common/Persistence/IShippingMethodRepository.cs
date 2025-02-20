using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

namespace Application.Common.Persistence;

/// <summary>
/// Defines the contract for shipping method persistence operations.
/// </summary>
public interface IShippingMethodRepository : IBaseRepository<ShippingMethod, ShippingMethodId>
{
}
